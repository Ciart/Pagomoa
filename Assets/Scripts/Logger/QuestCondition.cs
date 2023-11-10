using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Logger
{
    [Serializable] 
    public class QuestCondition<T>
    {
        public string Summary { get; set; }
        public T Value { get; set; }
        public ScriptableObject targetObject;

        protected QuestCondition(string summary, T value)
        {
            Summary = summary;
            Value = value;
        }
    }
    [Serializable]
    public class QuestFloatCondition : QuestCondition<float>
    {
        protected QuestFloatCondition(string summary, float value) : base(summary, value)
        {
            Summary = summary;
            Value = value;
        }
        public bool CompleteQuest()
        {
            return true;
        }
    }
    [Serializable]
    public class QuestBoolCondition : QuestCondition<bool>
    {
        protected QuestBoolCondition(string summary, bool value) : base(summary, value) 
        {
            Summary = summary;
            Value = value;
        }
        public bool CompleteQuest()
        {
            return Value;
        }
    }
}