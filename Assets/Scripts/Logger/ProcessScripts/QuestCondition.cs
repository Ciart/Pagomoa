using System;
using Ciart.Pagomoa.Events;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public abstract class QuestCondition
    {
        protected bool complete{ get; set; }
        protected float progress { get; set; }
        public Action questFinished;
        public QuestType questType { get; set; }
        protected string summary { get; set; }
        protected string targetId { get; set; }
        public string valueType { get; set; }

        protected int value { get; set; }
        protected int compareValue { get; set; }
        
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
