using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace Logger
{
    [Serializable] 
    public class QuestCondition
    {
        public string questType;
        public string Summary { get; set; }
        public string value;
        public ScriptableObject targetObject;
        
        public ConditionType questCondition = new ConditionType();
    }
}