using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.RefactoringManagerSystem
{

    public class QuestManager : Manager<QuestManager>
    {
        public List<Quest> quests = new List<Quest>();

        public QuestManager()
        {
            EventManager.AddListener<DataSaveEvent>(OnDataSaveEvent);
            EventManager.AddListener<DataLoadedEvent>(OnDataLoadedEvent);
        }

        public override void OnDestroy()
        {
            EventManager.RemoveListener<DataSaveEvent>(OnDataSaveEvent);
            EventManager.RemoveListener<DataLoadedEvent>(OnDataLoadedEvent);
        }

        private void OnDataSaveEvent(DataSaveEvent e)
        {
            e.saveData.quests = new List<QuestSaveData>();

            foreach (var quest in quests)
            {
                var questData = new QuestSaveData
                {
                    id = quest.id,
                    state = quest.state,
                    conditions = new QuestConditionSaveData[quest.conditions.Count]
                };

                for (var i = 0; i < quest.conditions.Count; i++)
                {
                    questData.conditions[i] = new QuestConditionSaveData
                    {
                        compareValue = quest.conditions[i].GetCompareValue()
                    };
                }

                e.saveData.quests.Add(questData);
            }
        }

        private void OnDataLoadedEvent(DataLoadedEvent e)
        {
            quests = new List<Quest>();

            foreach (var questData in e.saveData.quests)
            {
                var quest = new Quest(ResourceSystem.Instance.GetQuest(questData.id));
                quest.state = questData.state;

                for (var i = 0; i < questData.conditions.Length; i++)
                {
                    quest.conditions[i].SetCompareValue(questData.conditions[i].compareValue);
                }

                quests.Add(quest);
            }
        }

        public void RegistrationQuest(string entityId, string questId)
        {
            var targetQuests = ResourceSystem.Instance.GetQuests(entityId);

            foreach (var quest in targetQuests)
            {
                if (quest.id != questId) continue;

                var progressQuest = new Quest(quest);

                quests.Add(progressQuest);

                EventManager.Notify(new QuestStarted(progressQuest));
                // EventManager.Notify(new AddNpcImageEvent(npcSprite));
                EventManager.Notify(new MakeQuestListEvent());
            }

            EventManager.Notify(new QuestListUpdated(quests));

            if (Game.Instance.Time.IsTutorialDay)
            {
                SaveSystem.Instance.Save();
            }
        }

        public void CompleteQuest(string id)
        {
            GetReward(id);

            EventManager.Notify(new QuestCompleted(FindQuestById(id)));

            if (Game.Instance.Time.IsTutorialDay)
            {
                SaveSystem.Instance.Save();
            }
        }

        private void GetReward(string id)
        {
            var targetQuest = FindQuestById(id);
            var reward = targetQuest.reward;

            EventManager.Notify(new AddReward(reward.itemID, reward.value));

            EventManager.Notify(new AddGold(reward.gold));

            targetQuest.state = QuestState.Finish;
            targetQuest.Finish();
        }

        public Quest? FindQuestById(string id)
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
            }
            else
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
