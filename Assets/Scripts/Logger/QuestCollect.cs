using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Logger
{
    public class QuestCollect : QuestFloatCondition
    {
        public int targetCount;

        public QuestCollect(string summary, float value) : base(summary, value)
        {
            Summary = summary;
            Value = value;
        }

        public void CalculateMethod()
        {
            targetCount++;
        }
    }
}