using Logger.ForEditorBaseScripts;
using Logger.ProcessScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Logger.ProcessScripts
{
    public abstract class ProcessIntQuestElements : ProcessQuestElements, IProcessQuestValue<int>
    {
        public int value { get; set; }
        public int compareValue { get; set; }

        public void InitQuest(QuestType initQuestType, string initSummary, int initValue, int initCompareValue, ScriptableObject initTargetEntity)
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
        public virtual void CalculationValue() { }
    }

    public abstract class ProcessFloatQuestElements : ProcessQuestElements, IProcessQuestValue<float>
    {
        public float value { get; set; }
        public float compareValue { get; set; }

        ~ProcessFloatQuestElements()
        {
            
        }
        
        public void InitQuest(QuestType initQuestType, string initSummary, float initValue, float initCompareValue, ScriptableObject initTargetEntity)
        {
            questType = initQuestType;
            summary = initSummary;
            value = initValue;
            compareValue = initCompareValue;
            targetEntity = initTargetEntity;
        }
        
        public void InitQuestValue(float initValue, float initCompareValue)
        {
            value = initValue;
            compareValue = initCompareValue;
        }
        
        public void InitQuestValueWithTarget(float initValue, float initCompareValue, ScriptableObject initTargetEntity)
        {
            value = initValue;
            compareValue = initCompareValue;
            targetEntity = initTargetEntity;
        }
        
        public abstract void CalculationValue();
    }

    public abstract class ProcessBoolQuestElements : ProcessQuestElements, IProcessQuestValue<bool>
    {
        public bool value { get; set; }
        public bool compareValue { get; set; }
        
        ~ProcessBoolQuestElements()
        {
            
        }
        
        public void InitQuest(QuestType initQuestType, string initSummary, bool initValue, bool initCompareValue, ScriptableObject initTargetEntity)
        {
            questType = initQuestType;
            summary = initSummary;
            value = initValue;
            compareValue = initCompareValue;
            targetEntity = initTargetEntity;
        }
        
        public void InitQuestValue(bool initValue)
        {
            value = initValue;
            compareValue = !initValue;
        }
        
        public void InitQuestValueWithTarget(bool initValue, ScriptableObject initTargetEntity)
        {
            value = initValue;
            compareValue = !initValue;
            targetEntity = initTargetEntity;
        }
        public abstract void CalculationValue();
    }
    public interface IProcessQuestValue<T>
    {
        public T value { get; set; }
        public T compareValue { get; set; }
    }
    
    
}