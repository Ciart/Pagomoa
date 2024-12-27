using System;
using UnityEngine;

namespace Logger.ForEditorBaseScripts
{
    [Serializable] 
    public class QuestConditionData
    {
        public QuestType questType;
        
        public string summary;
        
        public string value;

        public String targetId;

        public ConditionType conditionType;
    }
}
