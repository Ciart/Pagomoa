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
        public string Summary { get; set; }
        public string value;
        public ScriptableObject targetObject;
        
        protected QuestCondition(string summary, string value)
        {
            Summary = summary;
            // 타입 설정
            // 벨류 결정
            // 타입에 따른 Conditiontype
        }
    }
}