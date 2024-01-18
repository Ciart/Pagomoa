using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logger
{
    #region MakeQuestPart
    public class ProcessQuest
    {
        public int questId;
        public int nextQuestId;
        public string description;
        public Reward reward;
        public List<ProcessQuestElements> elements;

        public ProcessQuest(int questId, int nextQuestId, string description, Reward reward, List<QuestCondition> questConditions)
        {
            this.questId = questId;
            this.nextQuestId = nextQuestId;
            this.description = description;
            this.reward = reward;
            elements = new List<ProcessQuestElements>();
            
            foreach (var condition in questConditions)
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
                }
            }
        }
    }

    public class ProcessIntQuestElements : ProcessQuestElements, IProcessQuestValue<int>
    {
        public int value { get; set; }
        public ProcessIntQuestElements(QuestType questType, string summary, int value, ScriptableObject targetEntity) 
            : base(questType, summary, targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
            valueType = "int";
        }
    }

    public class ProcessFloatQuestElements : ProcessQuestElements, IProcessQuestValue<float>
    {
        public float value { get; set; } 
        public ProcessFloatQuestElements(QuestType questType, string summary, float value, ScriptableObject targetEntity) 
            : base(questType, summary, targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
            valueType = "float";
        }
    }
    
    public class ProcessBoolQuestElements : ProcessQuestElements, IProcessQuestValue<bool>
    {
        public bool value { get; set; }
        public ProcessBoolQuestElements(QuestType questType, string summary, bool value, ScriptableObject targetEntity) 
            : base(questType, summary, targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
            valueType = "bool";
        }
    }

    public abstract class ProcessQuestElements
    {
        protected ProcessQuestElements(QuestType questType, string summary , ScriptableObject targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.targetEntity = targetEntity;
        }

        public QuestType questType { get; set; }
        public string summary { get; set; }
        public ScriptableObject targetEntity { get; set; }
        public string valueType { get; set; }
    }

    public interface IProcessQuestValue<T>
    {
        public T value { get; set; }
    }
    #endregion

    [Serializable]
    [RequireComponent(typeof(QuestDatabase))]
    public class QuestManager : MonoBehaviour
    {
        public List<ProcessQuest> progressQuests = new List<ProcessQuest>();
        public QuestDatabase database;
        
        private static QuestManager _instance;
        public static QuestManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance =  (QuestManager)FindObjectOfType(typeof(QuestManager));
                }
                return _instance;
            }
        }

        private void Start()
        {
            database = GetComponent<QuestDatabase>();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                MakeQuest(1);
            }
        }

        public void MakeQuest(int questId)
        {
            // todo : UI로 적용이 필요함

            foreach (var quest in database.quests)
            {
                if (quest.questId == questId)
                {
                    var q = new ProcessQuest(quest.questId, quest.nextQuestId, quest.description, quest.reward, quest.questList);
                    
                    progressQuests.Add(q);
                }
            }

            foreach (var progressQuest in progressQuests)
            {
                Debug.Log("ID");
                Debug.Log("questId: " + progressQuest.questId);
                Debug.Log("nextQuestId: " + progressQuest.nextQuestId);
                
                Debug.Log("Reward");
                Debug.Log("gold: " + progressQuest.reward.gold);
                Debug.Log("targetEntity: " + progressQuest.reward.targetEntity);
                Debug.Log("EntityValue: " + progressQuest.reward.value);
                
                for (int i = 0; i < progressQuest.elements.Count; i++)
                {
                    Debug.Log("QuestType: " + progressQuest.elements[i].questType);
                    Debug.Log("Summary: " + progressQuest.elements[i].summary);
                    Debug.Log("ValueType: " + progressQuest.elements[i].valueType);
                    Debug.Log("Target: " + progressQuest.elements[i].targetEntity);
                    if (progressQuest.elements[i].GetType() == typeof(ProcessIntQuestElements))
                    {
                        var intVal = (ProcessIntQuestElements)progressQuest.elements[i];
                        Debug.Log("intValue: " + intVal.value);
                    }
                    else if (progressQuest.elements[i].GetType() == typeof(ProcessFloatQuestElements))
                    {
                        var floatVal = (ProcessFloatQuestElements)progressQuest.elements[i];
                        Debug.Log("floatValue: " + floatVal.value);
                    }
                    else if (progressQuest.elements[i].GetType() == typeof(ProcessBoolQuestElements))
                    {
                        var boolVal = (ProcessBoolQuestElements)progressQuest.elements[i];
                        Debug.Log("boolValue: " + boolVal.value);
                    }
                }
            }
        }
    }   
}
