using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using Logger;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Logger
{
    public delegate void RegisterQuest(InteractableObject questInCharge, int id);
    public delegate void CompleteQuest(InteractableObject questInCharge, int id);

    [Serializable]
    [RequireComponent(typeof(QuestDatabase))]
    public class QuestManager : MonoBehaviour
    {
        public List<ProcessQuest> progressQuests = new List<ProcessQuest>();
        public QuestDatabase database;
        
        public RegisterQuest registerQuest;
        public CompleteQuest completeQuest;
        
        private static QuestManager _instance;
        public static QuestManager instance
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
            database ??= GetComponent<QuestDatabase>();

            registerQuest = RegistrationQuest;
            completeQuest = CompleteQuest;
        }

        public void RegistrationQuest(InteractableObject questInCharge, int questId)
        {
            foreach (var quest in database.quests)
            {
                if (quest.questId == questId)
                {
                    var q = new ProcessQuest(quest.questId, quest.nextQuestId, quest.description, quest.title, quest.reward, quest.questList)
                        {
                            questInCharge = questInCharge
                        };
                           
                    progressQuests.Add(q);
                    
                    EventManager.AddListener<SignalToNpc>(QuestAccomplishment);
                    
                    if (QuestUI.instance != null)
                    {
                        QuestUI.instance.npcImages.Add(questInCharge.GetComponent<SpriteRenderer>().sprite);
                        QuestUI.instance.MakeProgressQuestList();
                    }
                }
            }
        }
        
        private void QuestAccomplishment(SignalToNpc e)
        {
            var isQuestComplete = e.accomplishment;
            var questInCharge = e.questInCharge;
            var dialogueTrigger = e.questInCharge.GetComponent<DialogueTrigger>();

            if (isQuestComplete == false)
            {
                questInCharge.transform.GetChild(1).gameObject.SetActive(false);
                
                // todo : 유효성 검사 
                questInCharge.interactionEvent.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
                questInCharge.interactionEvent.RemoveListener( () => dialogueTrigger.QuestCompleteDialogue(e.questId));
                questInCharge.interactionEvent.AddListener(dialogueTrigger.StartStory);
            }
            else
            {
                questInCharge.transform.GetChild(1).gameObject.SetActive(true);
                questInCharge.interactionEvent.SetPersistentListenerState(0, UnityEventCallState.Off);
                questInCharge.interactionEvent.AddListener(() => dialogueTrigger.QuestCompleteDialogue(e.questId));
            }
        }
        
        private void CompleteQuest(InteractableObject questInCharge, int questId)
        {
            var dialogueTrigger = questInCharge.GetComponent<DialogueTrigger>();
            
            questInCharge.transform.GetChild(1).gameObject.SetActive(false);
            questInCharge.interactionEvent.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
            questInCharge.interactionEvent.RemoveListener(() => dialogueTrigger.QuestCompleteDialogue(questId));
            
            GetReward(questInCharge, questId);
        }
        
        private void GetReward(InteractableObject questInCharge, int questId)
        {
            Debug.Log("리워드 지급");

            var targetQuest = FindQuestByID(questId);
            var reward = targetQuest.reward;

            InventoryDB.Instance.Add((Item)reward.targetEntity, reward.value);
            InventoryDB.Instance.Gold += reward.gold;
        }

        private ProcessQuest FindQuestByID(int questId)
        {
            foreach (var quest in progressQuests)
            {
                if (quest.questId == questId)
                {
                    return quest;
                }
            }
            
            return null;
        }
    }   
}
