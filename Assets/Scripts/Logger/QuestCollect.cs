using System.Collections.Generic;
using UnityEngine;

namespace Logger
{
    public class QuestCollect : QuestType
    {
        public Item targetObject;
        public int targetCount;

        public QuestCollect(string summary, int value) : base(summary, value)
        {
            this.summary = base.summary;
            intValue = value;
        }

        public bool CompleteQuest()
        {
            return targetCount == intValue;
        }

        public void CalculateMethod()
        {
            targetCount++;
        }
    }
}