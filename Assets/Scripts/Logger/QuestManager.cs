using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
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
                questInCharge.interactionEvent.RemoveListener( () => dialogueTrigger.QuestCompleteDialogue(e.questId));
                questInCharge.interactionEvent.AddListener(dialogueTrigger.StartStory);
            }
            else
            {
                questInCharge.transform.GetChild(1).gameObject.SetActive(true);
                questInCharge.interactionEvent.AddListener(() => dialogueTrigger.QuestCompleteDialogue(e.questId));
            }
        }
        
        private void CompleteQuest(InteractableObject questInCharge, int id)
        {
            var dialogueTrigger = questInCharge.GetComponent<DialogueTrigger>();
            
            questInCharge.transform.GetChild(1).gameObject.SetActive(false);
            questInCharge.interactionEvent.RemoveListener(() => dialogueTrigger.QuestCompleteDialogue(id));
            questInCharge.interactionEvent.AddListener(() => dialogueTrigger.StartStory());

            GetReward(id);
        }
        
        private void GetReward(int id)
        {
            var targetQuest = FindQuestById(id);
            var reward = targetQuest.reward;

            InventoryDB.Instance.Add((Item)reward.targetEntity, reward.value);
            InventoryDB.Instance.Gold += reward.gold;
            
            // todo : 완료된 퀘스트를 저장 할까 말까?
            database.progressedQuests.Add(new ProgressedQuest(targetQuest));
            progressQuests.Remove(targetQuest);
        }
        
        public ProcessQuest FindQuestById(int id)
        {
            foreach (var quest in progressQuests)
            {
                if (quest.questId == id) return quest;
            }
            
            return null;
        }

        public bool CheckQuestCompleteById(int id)
        {
            foreach (var quest in progressQuests)
            {
                return FindQuestById(id) is not null && quest.accomplishment;
            }
            
            return false;
        }
        
        // 경훈아 이거쓰셈
        public bool IsRegisteredQuest(int id)
        {
            var check = false;
            
            foreach (var quest in progressQuests)
            {
                if (quest.questId == id) check = true;
            }

            return check;
        }

        public bool IsCompleteQuest(int id)
        {
            var check = false;
            
            foreach (var quest in database.progressedQuests)
            {
                if (quest.questId == id) check = true;
            }

            return check;
        }
    }   
}
