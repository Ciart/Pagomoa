using System.Collections.Generic;
using UnityEngine;

namespace Logger
{
    #region IntQuestCondition
    public class CollectMineral : ProcessIntQuestElements
    {
        public CollectMineral(QuestType questType, string summary, int value, ScriptableObject targetEntity) : base(questType, summary, value, targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
            valueType = "int";
            compareValue = 0;
        }
    }
    
    public class ConsumeMineral : ProcessIntQuestElements
    {
        public ConsumeMineral(QuestType questType, string summary, int value, ScriptableObject targetEntity) : base(questType, summary, value, targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
            valueType = "int";
            compareValue = 0;
        }
    }
    #endregion
    
    #region FloatQuestCondition
    
    #endregion
    
    #region BoolQuestCondition
    
    #endregion
    
    # region ParentSource

    public abstract class ProcessIntQuestElements : ProcessQuestElements, IProcessQuestValue<int>
    {
        public int value { get; set; }
        public int compareValue { get; set; }

        /*protected ProcessIntQuestElements(QuestType questType, string summary, int value, ScriptableObject targetEntity)
            : base(questType, summary, targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
            valueType = "int";
            compareValue = 0;
        }*/

        ~ProcessIntQuestElements()
        {
            // todo 소멸자
        }

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

        public virtual bool CheckComplete()
        {
            return compareValue == value;
        }
        
        public virtual void CalculationValue() { }
    }

    public abstract class ProcessFloatQuestElements : ProcessQuestElements, IProcessQuestValue<float>
    {
        public float value { get; set; }
        public float compareValue { get; set; }

        /*protected ProcessFloatQuestElements(QuestType questType, string summary, float value,
            ScriptableObject targetEntity)
            : base(questType, summary, targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
            valueType = "float";
        }*/

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
        
        public virtual bool CheckComplete()
        {
            return compareValue >= value;
        }
        
        public virtual void CalculationValue() { }
    }

    public abstract class ProcessBoolQuestElements : ProcessQuestElements, IProcessQuestValue<bool>
    {
        public bool value { get; set; }
        public bool compareValue { get; set; }

        /*protected ProcessBoolQuestElements(QuestType questType, string summary, bool value, ScriptableObject targetEntity)
            : base(questType, summary, targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
            valueType = "bool";
            compareValue = !value;
        }*/

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
        
        public virtual bool CheckComplete()
        {
            return compareValue == value;
        }
        
        public virtual void CalculationValue() { }
    }

    public abstract class ProcessQuestElements
    {
        /*protected ProcessQuestElements(QuestType questType, string summary, ScriptableObject targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.targetEntity = targetEntity;
        }*/

        public QuestType questType { get; set; }
        public string summary { get; set; }
        public ScriptableObject targetEntity { get; set; }
        public string valueType { get; set; }
    }

    public interface IProcessQuestValue<T>
    {
        public T value { get; set; }
        public T compareValue { get; set; }
    }

    # endregion

    public class ProcessQuest
    {
        public int questId;
        public int nextQuestId;
        public string description;
        public Reward reward;
        public List<ProcessQuestElements> elements;

        public ProcessQuest(int questId, int nextQuestId, string description, Reward reward,
            List<QuestCondition> questConditions)
        {
            this.questId = questId;
            this.nextQuestId = nextQuestId;
            this.description = description;
            this.reward = reward;
            elements = new List<ProcessQuestElements>();

            /*foreach (var condition in questConditions)
            {
                if (condition.conditionType.typeValue == "int")
                {
                    var intValue = int.Parse(condition.value);
                    
                    var element = new ProcessIntQuestElements(
                        condition.questType,
                        condition.summary,
                        intValue,
                        condition.targetEntity
                    );
                    
                    elements.Add(element);
                }
                else if (condition.conditionType.typeValue == "float")
                {
                    var floatValue = float.Parse(condition.value);
                    
                    var element = new ProcessFloatQuestElements(
                        condition.questType,
                        condition.summary,
                        floatValue,
                        condition.targetEntity
                    );
                    
                    elements.Add(element);
                }
                else if (condition.conditionType.typeValue == "bool")
                {
                    var boolValue = bool.Parse(condition.value);
                    
                    var element = new ProcessBoolQuestElements(
                        condition.questType,
                        condition.summary,
                        boolValue,
                        condition.targetEntity
                    );
                    
                    elements.Add(element);
                }*/
        }
    }
}