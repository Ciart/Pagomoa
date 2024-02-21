using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Logger.ProcessScripts;

using UnityEngine;

namespace Logger
{
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
            
        }

        public void MakeQuest(int questId)
        {
            foreach (var quest in database.quests)
            {
                if (quest.questId == questId)
                {
                    var q = new ProcessQuest(quest.questId, quest.nextQuestId, quest.description, quest.reward, quest.questList);
                    
                    progressQuests.Add(q);
                }
            }
            
            /*foreach (var progressQuest in progressQuests)
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
                    if (progressQuest.elements[i] is ProcessIntQuestElements)
                    {
                        var intVal = (ProcessIntQuestElements)progressQuest.elements[i];
                        Debug.Log("intValue: " + intVal.value);
                    }
                    else if (progressQuest.elements[i] is ProcessFloatQuestElements)
                    {
                        var floatVal = (ProcessFloatQuestElements)progressQuest.elements[i];
                        Debug.Log("floatValue: " + floatVal.value);
                    }
                    else if (progressQuest.elements[i] is ProcessBoolQuestElements)
                    {
                        var boolVal = (ProcessBoolQuestElements)progressQuest.elements[i];
                        Debug.Log("boolValue: " + boolVal.value);
                        
                        CompleteBoolQuest(questId, boolVal.value);
                    }
                }
            }*/
        }
    }   
}
