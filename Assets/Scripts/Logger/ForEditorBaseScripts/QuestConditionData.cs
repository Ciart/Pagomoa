using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logger.ForEditorBaseScripts
{
    [Serializable] 
    public class QuestConditionData
    {
        public QuestType questType;
        
        public string summary = "";
        
        public string value = "";

        public string targetID = "";

        public ConditionType conditionType;
    }
}
