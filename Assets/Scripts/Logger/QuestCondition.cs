using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Logger
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