using UnityEngine;

namespace Logger
{
    public class QuestEncounter : QuestType
    {
        public Item targetObject;

        public QuestEncounter(string summary, bool value) : base(summary, value)
        {
            this.summary = summary;
            this.boolValue = value;
        }

        public bool CompleteQuest()
        {
            throw new System.NotImplementedException();
        }

        public void CalculateMethod()
        {
            throw new System.NotImplementedException();
        }
    }
}