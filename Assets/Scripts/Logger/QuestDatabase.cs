using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Logger.ProcessScripts;
using UnityEngine;


namespace Logger
{
    public class QuestDatabase : MonoBehaviour
    {
        public readonly List<ProgressedQuest> progressedQuests = new List<ProgressedQuest>();

        public List<EntityOrigin> entities;

        [Serializable]
        public class MapEntityQuest
        {
            public string entityName = "";
            public List<Quest> entityQuests = new List<Quest>();
        }

        public List<MapEntityQuest> mapEntityQuests;

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

        public Quest[] GetEntityQuestsByEntityID(string id)
        {
            foreach (var mapEntity in mapEntityQuests)
            {
                if (mapEntity.entityName == id)
                {
                    return mapEntity.entityQuests.ToArray();
                }
            }

            return null;
        }
    }
}