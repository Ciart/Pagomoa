using System;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.RefactoringManagerSystem;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;


namespace Logger
{
    public class QuestDatabase : MonoBehaviour
    {
        [Serializable]
        public class MapEntityQuest
        {
            public EntityController entity;
            public List<QuestData> entityQuests = new List<QuestData>();
        }

        public List<MapEntityQuest> mapEntityQuests = new List<MapEntityQuest>();

        public QuestData[] GetEntityQuestsByEntity(string entityId)
        {
            foreach (var mapEntity in mapEntityQuests)
            {
                if (mapEntity.entity.entityId == entityId)
                {
                    return mapEntity.entityQuests.ToArray();
                }
            }

            return Array.Empty<QuestData>();
        }
    }
}