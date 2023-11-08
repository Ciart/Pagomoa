using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Logger
{
    public class QuestType
    {
        public string summary;
        public int intValue;
        public float floatValue;
        public bool boolValue;

        protected QuestType(string summary, int value)
        {
            this.summary = summary;
            this.intValue = value;
        }
        
        protected QuestType(string summary, float value)
        {
            this.summary = summary;
            this.floatValue = value;
        }

        protected QuestType(string summary, bool value)
        {
            this.summary = summary;
            this.boolValue = value;
        }
    }
}