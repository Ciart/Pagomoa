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
        
        public ProgressedQuest FindQuestById(int id)
        {
            foreach (var quest in progressedQuests)
            {
                if (quest.id == id) return quest;
            }
            
            return null;
        }

        public bool CheckQuestCompleteById(int id)
        {
            foreach (var quest in progressedQuests)
            {
                return FindQuestById(id) is not null && quest.accomplishment;
            }
            
            return false;
        }
    }
}