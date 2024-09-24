using Ciart.Pagomoa.Events;
using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public abstract class QuestCondition : ProcessQuestElements
    {
        public void InitQuest(QuestType initQuestType, string initSummary, int initValue, int initCompareValue,
            ScriptableObject initTargetEntity)
        {
            questType = initQuestType;
            summary = initSummary;
            value = initValue;
            compareValue = initCompareValue;
            targetEntity = initTargetEntity;
        }

        public void InitQuestValue(int initValue, int initCompareValue)
        {
            value = initValue;
            compareValue = initCompareValue;
        }

        public void InitQuestValueWithTarget(int initValue, int initCompareValue, ScriptableObject initTargetEntity)
        {
            value = initValue;
            compareValue = initCompareValue;
            targetEntity = initTargetEntity;
        }

        public virtual void CalculationValue(IEvent e)
        {
        }
    }
}