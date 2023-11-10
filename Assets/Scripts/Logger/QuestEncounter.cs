using UnityEngine;

namespace Logger
{
    public class QuestEncounter : QuestBoolCondition
    {
        public Item targetObject;

        public QuestEncounter(string summary, bool value) : base(summary, value)
        {
            Summary = summary;
            Value = value;
        }

        public void CalculateMethod()
        {
            throw new System.NotImplementedException();
        }
    }
}