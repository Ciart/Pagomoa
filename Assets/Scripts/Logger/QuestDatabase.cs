using System.Collections.Generic;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Logger
{
    public class QuestDatabase : MonoBehaviour
    {
        public List<Quest> quests = new List<Quest>();

        public List<ProgressedQuest> progressedQuests = new List<ProgressedQuest>();
        
        private ProgressedQuest FindQuestById(string id)
        {
            foreach (var quest in progressedQuests)
            {
                if (quest.id == id) return quest;
            }
            
            return null;
        }

        public bool CheckQuestCompleteById(string id)
        {
            foreach (var quest in progressedQuests)
            {
                return FindQuestById(id) is not null && quest.accomplishment;
            }
            
            return false;
        }
    }
}