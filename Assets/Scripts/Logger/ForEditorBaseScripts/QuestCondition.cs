using System;
using UnityEngine;

namespace Logger.ForEditorBaseScripts
{
    [Serializable] 
    public class QuestCondition
    {
        public QuestType questType;
        
        public string summary;
        
        public string value;

        public ScriptableObject targetEntity;

        public ConditionType conditionType;
    }
}