using System.Collections.Generic;
using UnityEngine;
using Worlds;

namespace Logger
{
    public enum QuestType {
        CollectMineral,
        ConsumeMineral,
        BreakBlock,
        EnCounterMineral,
        EncounterNpc,
    }
    
    public class ConditionType
    {
        public QuestType type;
        public ScriptableObject targetType;
        public int valueInt;
        public int valueFloat;
        public int valueBool;

        private Dictionary<QuestType, ConditionType> TypeList = new Dictionary<QuestType, ConditionType>()
        {
            { QuestType.CollectMineral, new ConditionType(QuestType.CollectMineral) },
        };

        public ConditionType(QuestType type)
        {
            if (type == QuestType.CollectMineral)
            {
                
            }
        }


        private void Add(QuestType collectMineral)
        {
            throw new System.NotImplementedException();
        }
    }
}
