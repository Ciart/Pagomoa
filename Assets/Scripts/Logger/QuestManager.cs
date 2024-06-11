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

namespace Ciart.Pagomoa.Logger
{
    [Serializable]
    [RequireComponent(typeof(QuestDatabase))]
    public class QuestManager : MonoBehaviour
    {
        [Header("수행중인 퀘스트")]
        public List<ProcessQuest> progressQuests = new List<ProcessQuest>();
        
        public QuestDatabase database;

        [Header("QuestComplete Prefab")] 
        public QuestCompleteIcon questFinishPrefab;

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
            
            EventManager.AddListener<QuestRegister>(RegistrationQuest);
            EventManager.AddListener<QuestValidation>(CheckQuestValidation);
        }
        
        public void RegistrationQuest(QuestRegister e)
        {
            
            foreach (var quest in database.quests)
            {
                if (quest.id != e.id) continue;
                
                EventManager.AddListener<SignalToNpc>(QuestAccomplishment);
                    
                var target = e.questInCharge;
                    
                var progressQuest = new ProcessQuest(quest, e.questInCharge);

                progressQuests.Add(progressQuest);

                if (QuestUI.instance != null)
                {
                    QuestUI.instance.npcImages.Add(target.GetComponent<SpriteRenderer>().sprite);
                    QuestUI.instance.MakeProgressQuestList();
                }
                    
                EventManager.Notify(new SignalToNpc(progressQuest.questId, progressQuest.accomplishment, progressQuest.questInCharge));
            }
        }
        
        public void QuestAccomplishment(SignalToNpc e)
        {
            var isQuestComplete = e.accomplishment;
            var questInCharge = e.questInCharge;
            var dialogueTrigger = e.questInCharge.GetComponent<EntityDialogue>();

            if (isQuestComplete)
            {
                questInCharge.transform.GetChild(1).gameObject.SetActive(false);

                // todo : 유효성 검사 
                //questInCharge.interactionEvent.RemoveListener( () => DialogueManager.instance.StartQuestCompleteChat(e.questId));
            }
            else
            {
                questInCharge.gameObject.SetActive(false);
                
                // todo : 이미 완료 상태로 대기중인 퀘스트가 있는지
            }
        }
        
        public void CompleteQuest(InteractableObject questInCharge, int id)
        {
            var dialogueTrigger = questInCharge.GetComponent<EntityDialogue>();

            questInCharge.transform.GetChild(1).gameObject.SetActive(false);
            //questInCharge.interactionEvent.RemoveListener(() => DialogueManager.instance.StartQuestCompleteChat(id));

            GetReward(id);
        }
        
        private void GetReward(int id)
        {
            var targetQuest = FindQuestById(id);
            var reward = targetQuest.reward;

            InventoryDB.Instance.Add((Item)reward.targetEntity, reward.value);
            InventoryDB.Instance.Gold += reward.gold;
            
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

        public void CheckQuestValidation(QuestValidation e)
        {
            if (!IsCompleteQuest(e.quest.prevQuestIds))
            {
                EventManager.Notify(new ValidationResult(false));
                return;
            }

            if (IsCompleteQuest(e.quest.id))
            {
                EventManager.Notify(new ValidationResult(false));
                return;
            }

            if (IsRegisteredQuest(e.quest.id))
            {
                EventManager.Notify(new ValidationResult(false));
                return;
            }
            
            EventManager.Notify(new ValidationResult(true)); 
        }
        
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
                if (quest.id == id) check = true;
            }

            return check;
        }

        public bool IsCompleteQuest(List<int> ids)
        {
            if (ids.Count == 0) return true;
            
            foreach (var id in ids)
            {
                if (!IsCompleteQuest(id)) return false;
            }

            return true;
        }
    }   
}
