using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Logger
{
    public class ProcessQuest
    {
        public int questId;
        public int nextQuestId;
        public string description;
        public ArrayList elements;

        public ProcessQuest(int questId, int nextQuestId, string description, List<QuestCondition> questConditions)
        {
            this.questId = questId;
            this.nextQuestId = nextQuestId;
            this.description = description;
            elements = new ArrayList();
            
            foreach (var condition in questConditions)
            {
                if (condition.conditionType.typeValue == "int")
                {
                    var intValue = int.Parse(condition.value);
                    
                    var element = new ProcessQuestElements<int>(
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
                    
                    var element = new ProcessQuestElements<float>(
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
                    
                    var element = new ProcessQuestElements<bool>(
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

    public class ProcessQuestElements<T>
    {
        public QuestType questType;
        public string summary;
        public T value;
        public ScriptableObject targetEntity;

        public ProcessQuestElements(QuestType questType, string summary, T value, ScriptableObject targetEntity)
        {
            this.questType = questType;
            this.summary = summary;
            this.value = value;
            this.targetEntity = targetEntity;
        }
    }

    [Serializable]
    public class QuestManager : MonoBehaviour
    {
        public List<ProcessQuest> processQuests = new List<ProcessQuest>();
        
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
            var questDataBase = QuestDatabase.Instance;
            
            foreach (var quest in questDataBase.quests)
            {
                if (quest.questId == questId)
                {
                    var q = new ProcessQuest(quest.questId, quest.nextQuestId, quest.description, quest.questList);
                    
                    processQuests.Add(q);
                }
            }
        }
    }   
}
