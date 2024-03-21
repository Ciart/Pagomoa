using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Logger;
using UnityEngine;

namespace Ciart.Pagomoa.Logger
{
    [Serializable]
    [RequireComponent(typeof(QuestDatabase))]
    public class QuestManager : MonoBehaviour
    {
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
            database = GetComponent<QuestDatabase>();
        }

        private void Update()
        {
            
        }

        public void MakeQuest(QuestEnroll inCharge, int questId)
        {
            foreach (var quest in database.quests)
            {
                if (quest.questId == questId)
                {
                    var q = new ProcessQuest(quest.questId, quest.nextQuestId, quest.title, quest.description, quest.reward, quest.questList);
                    
                    progressQuests.Add(q);
                }
            }
        }

        public void CompleteQuest()
        {
            
        }
    }   
}
