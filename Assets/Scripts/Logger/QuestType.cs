using System.Collections.Generic;
using UnityEngine;

namespace Logger
{
    public class QuestType : MonoBehaviour
    {
        protected string summary;
        protected int intValue;
        protected float floatValue;
        protected bool boolValue;

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