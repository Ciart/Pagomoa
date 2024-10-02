using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Logger;
using UnityEngine;

namespace Ciart.Pagomoa.Logger
{
    [Serializable]
    [RequireComponent(typeof(QuestDatabase))]
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        [Header("수행중인 퀘스트")]
        public List<Quest> quests = new List<Quest>();
        
        public QuestDatabase database;

        private void Start()
        {
            database ??= GetComponent<QuestDatabase>();
        }
        
        public void RegistrationQuest(Sprite npcSprite, EntityOrigin origin, string id)
        {
            var targetQuests = database.GetEntityQuestsByEntityID(origin);
            
            foreach (var quest in targetQuests)
            {
                if (quest.id != id) continue;
                
                var progressQuest = new Quest(quest, npcSprite);

                quests.Add(progressQuest);
                
                EventManager.Notify(new QuestStarted(progressQuest));
                EventManager.Notify(new AddNpcImageEvent(npcSprite));
                EventManager.Notify(new MakeQuestListEvent());
            }
            
            EventManager.Notify(new QuestListUpdated(quests));
        }

        public void CompleteQuest(string id)
        {
            GetReward(id);
            
            EventManager.Notify(new QuestCompleted(FindQuestById(id)));
        }
        
        private void GetReward(string id)
        {
            var targetQuest = FindQuestById(id);
            var reward = targetQuest.reward;

            EventManager.Notify(new AddReward((Item)reward.targetEntity, reward.value));
            EventManager.Notify(new AddGold(reward.gold));

            targetQuest.state = QuestState.Finish;
            
            targetQuest.Dispose();
        }
        
        public Quest FindQuestById(string id)
        {
            foreach (var quest in quests)
            {
                if (quest.id == id) return quest;
            }
            
            return null;
        }

        public bool CheckQuestValidation(QuestData questData)
        { 
            var prevQuestCount = questData.prevQuestIds.Count;
            
            if (prevQuestCount != 0)
            {
                if (quests.Count == 0) return false;
                
                foreach (var quest in quests)
                {
                    foreach (var questId in questData.prevQuestIds)
                    {
                        if (quest.id != questId) continue;
                        
                        if (quest.state != QuestState.Finish) return false;
                        prevQuestCount--;
                    }
                    
                    if (prevQuestCount != 0) return false;
                    if (quest.id == questData.id) return false;
                }
            } else
            {
                foreach (var quest in quests)
                {
                    if (quest.id == questData.id) return false;
                }
            }

            return true;
        }

        public bool FindCompletedQuest(QuestData[] questData)
        {
            foreach (var data in questData)
            {
                foreach (var quest in quests)
                {
                    if (quest.id == data.id && quest.state == QuestState.Completed)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }   
}
