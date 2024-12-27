using System;
using Ciart.Pagomoa.Events;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public abstract class QuestCondition
    {
        public bool complete{ get; set; }
        public float progress { get; set; }
        public Action questFinished;
        public QuestType questType { get; set; }
        public string summary { get; set; }
        public string targetId { get; set; }
        public string valueType { get; set; }

        public int value { get; set; }
        public int compareValue { get; set; }
        
        public void InitQuest(QuestType initQuestType, string initSummary, int initValue, int initCompareValue,
            string initTargetId)
        {
            questType = initQuestType;
            summary = initSummary;
            value = initValue;
            compareValue = initCompareValue;
            targetId = initTargetId;
        }

        public void InitQuestValue(int initValue, int initCompareValue)
        {
            value = initValue;
            compareValue = initCompareValue;
        }

        public void InitQuestValueWithTarget(int initValue, int initCompareValue, string initTargetId)
        {
            value = initValue;
            compareValue = initCompareValue;
            targetId = initTargetId;
        }

        public abstract void CalculationValue(IEvent e);
        public abstract bool TypeValidation(string target);
    }
}
