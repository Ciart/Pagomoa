using System;
using System.Collections.Generic;
using System.Dynamic;
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
            EventManager.AddListener<CompleteQuest>(CompleteQuest);
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
                    
                EventManager.Notify(new SignalToNpc(progressQuest.id, progressQuest.accomplishment, progressQuest.questInCharge));
            }
        }
        
        public void QuestAccomplishment(SignalToNpc e)
        {
            var signalID = e.questId;
            var isQuestComplete = e.accomplishment;
            var questInCharge = e.questInCharge;
            
            if (isQuestComplete)
            {
                questInCharge.ActiveQuestCompleteUI();
                
                EventManager.Notify(new SetCompleteChat(signalID));
            }
            else
            {
                WaitingForCompletedQuest(questInCharge);
            }
        }
        
        public void CompleteQuest(CompleteQuest e)
        {
            GetReward(e.id);
            
            WaitingForCompletedQuest(e.questInCharge);
        }
        
        private void GetReward(string id)
        {
            var targetQuest = FindQuestById(id);
            var reward = targetQuest.reward;
            
            InventoryDB.Instance.Add((Item)reward.targetEntity, reward.value);
            InventoryDB.Instance.Gold += reward.gold;
            
            database.progressedQuests.Add(new ProgressedQuest(targetQuest));
            progressQuests.Remove(targetQuest);
            
            targetQuest.Dispose();
        }
        
        private ProcessQuest FindQuestById(string id)
        {
            foreach (var quest in progressQuests)
            {
                if (quest.id == id) return quest;
            }
            
            return null;
        }

        private void CheckQuestValidation(QuestValidation e)
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

        private void WaitingForCompletedQuest(InteractableObject questInCharge)
        {
            var dialogue = questInCharge.GetComponent<EntityDialogue>();
            
            foreach (var quest in dialogue.entityQuests)
            {
                foreach (var processQuest in progressQuests)
                { 
                    if (processQuest.accomplishment && processQuest.id == quest.id)
                    {
                        EventManager.Notify(new SetCompleteChat(processQuest.id));
                        return;
                    }
                }
            }
            
            questInCharge.DeActiveQuestCompleteUI();
        }
        
        private bool IsRegisteredQuest(string id)
        {
            var check = false;
            
            foreach (var quest in progressQuests)
            {
                if (quest.id == id) check = true;
            }

            return check;
        }

        private bool IsCompleteQuest(string id)
        {
            var check = false;
            
            foreach (var quest in database.progressedQuests)
            {
                if (quest.id == id) check = true;
            }

            return check;
        }

        private bool IsCompleteQuest(List<string> ids)
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
