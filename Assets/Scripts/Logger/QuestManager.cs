using System;
using System.Collections.Generic;
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
            }
        }

        public bool CompleteBoolQuest(int id, bool completeVal)
        {
            // npc나 특정지역이면 collider를 트리거 
            // => npc에게 퀘스트 이벤트를 쥐어주고 npc가 가진 콜라이더가 퀘스트 매니저에게 이벤트 선언
            // 아이템이면 인벤토리에 존재 유무를 통하여 결정 

            for (int i = 0; i < progressQuests.Count; i++)
            {
                if (progressQuests[i].questId.Equals(id))
                {
                    foreach (var quest in progressQuests[i].elements)
                    {
                        if (progressQuests[i].GetType() == typeof(ProcessBoolQuestElements))
                        {
                            var items = InventoryDB.Instance.items;
                        
                            foreach (var item in items)
                            {
                                if (progressQuests[i].elements[i].targetEntity == item.item)
                                {
                                    Debug.Log($"정상적으로 잘 돌아가네여 : {progressQuests[i].elements[i].targetEntity}, {item.item}");
                                    return true;
                                }
                            }    
                        }
                    }

                    return false;
                }
            }
            
            return false;
        }

        public bool CompleteIntQuest(bool completeVal)
        {
            // +- 연산 필요
            // 골드 연산 따로
            // 아이템이면 인벤토리를 통하여 결정 
            // => 인벤토리의 엔티티 개수변경때 호출하여
            // => 인벤토리의 엔티티 값과 퀘스트 목적값이 같으면 완료
            
            return true;
        }
    }   
}
