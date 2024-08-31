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
    public class QuestManager : MonoBehaviour
    {
        [Header("수행중인 퀘스트")]
        public List<ProcessQuest> progressQuests = new List<ProcessQuest>();
        
        public QuestDatabase database;

        private static QuestManager _instance;
        public static QuestManager instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance =  (QuestManager)FindObjectOfType(typeof(QuestManager));
                }
                return _instance;
            }
        }

        private void Start()
        {
            database ??= GetComponent<QuestDatabase>();
        }
        
        public void RegistrationQuest(Sprite entitySprite, EntityOrigin origin, string id)
        {
            var targetQuests = database.GetEntityQuestsByEntityID(origin);
            
            foreach (var quest in targetQuests)
            {
                if (quest.id != id) continue;
                
                EventManager.AddListener<SignalToNpc>(QuestAccomplishment);
                
                var progressQuest = new ProcessQuest(quest);

                progressQuests.Add(progressQuest);
                
                EventManager.Notify(new AddNpcImageEvent(entitySprite));
                EventManager.Notify(new MakeQuestListEvent());
              
                EventManager.Notify(new SignalToNpc(progressQuest.id, progressQuest.accomplishment));
                
            }
            
            EventManager.Notify(new QuestListUpdated(progressQuests));
        }
        
        public void QuestAccomplishment(SignalToNpc e)
        {
            EventManager.Notify(new CompleteQuestsUpdated(GetCompleteQuests()));
        }
        
        public void CompleteQuest(string id)
        {
            GetReward(id);
            
            EventManager.Notify(new CompleteQuestsUpdated(GetCompleteQuests()));
        }
        
        private void GetReward(string id)
        {
            var targetQuest = FindQuestById(id);
            var reward = targetQuest.reward;

            EventManager.Notify(new AddReward((Item)reward.targetEntity, reward.value));
            EventManager.Notify(new AddGold(reward.gold));
            
            database.progressedQuests.Add(new ProgressedQuest(targetQuest));
            progressQuests.Remove(targetQuest);
            
            targetQuest.Dispose();
        }
        
        public ProcessQuest FindQuestById(string id)
        {
            foreach (var quest in progressQuests)
            {
                if (quest.id == id) return quest;
            }
            
            return null;
        }

        public bool CheckQuestValidation(Quest quest)
        {
            if (!IsCompletedQuest(quest.prevQuestIds))
            {
                return false;
            }
                
            if (IsCompletedQuest(quest.id))
            {
                return false;
            }

            if (IsRegisteredQuest(quest.id))
            {
                return false;
            }

            return true;
        }

        private bool IsRegisteredQuest(string id)
        {
            var check = false;
            
            foreach (var quest in progressQuests)
            {
                if (quest.id == id) check = true;
            }

            return check;
        }

        private bool IsCompletedQuest(string id)
        {
            var check = database.CheckQuestCompleteById(id);

            return check;
        }

        private bool IsCompletedQuest(List<string> ids)
        {
            if (ids.Count == 0) return true;
            
            foreach (var id in ids)
            {
                if (!IsCompletedQuest(id)) return false;
            }
                
            return true;
        }

        private ProcessQuest[] GetCompleteQuests()
        {
            var completeQuest = new List<ProcessQuest>();

            foreach (var quest in progressQuests)
            {
                if (quest.accomplishment) completeQuest.Add(quest);
            }

            return completeQuest.ToArray();
        }
    }   
}
