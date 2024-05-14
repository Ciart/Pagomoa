using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Inventory;
using Ink.Parsed;
using Logger;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Ciart.Pagomoa.Logger
{
    public delegate void RegisterQuest(DialogueTrigger questInCharge, int id);
    public delegate void CompleteQuest(DialogueTrigger questInCharge, int id);

    [Serializable]
    [RequireComponent(typeof(QuestDatabase))]
    public class QuestManager : MonoBehaviour
    {
        [Header("수행중인 퀘스트")]
        public List<ProcessQuest> progressQuests = new List<ProcessQuest>();
        
        public QuestDatabase database;

        [Header("QuestComplete Prefab")] 
        public QuestCompleteIcon questFinishPrefab; 
        
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
        
        public void RegistrationQuest(DialogueTrigger questInCharge, int questId)
        {
            foreach (var quest in database.quests)
            {
                if (quest.id == questId)
                {
                    EventManager.AddListener<SignalToNpc>(QuestAccomplishment);
                    
                    var target = questInCharge.GetComponent<InteractableObject>();

                    var targetTransform = questInCharge.transform;
                    
                    var completeIcon = 
                        Instantiate(
                            questFinishPrefab
                            , targetTransform.GetChild(0).transform.position + new Vector3(0, 1.3f, 0)
                            , Quaternion.identity, targetTransform);
                    
                    completeIcon.questId = questId;

                    var q = new ProcessQuest(target, quest.id, 1, quest.description, quest.title,
                        quest.reward, quest.questList);

                    progressQuests.Add(q);

                    if (QuestUI.instance != null)
                    {
                        QuestUI.instance.npcImages.Add(questInCharge.GetComponent<SpriteRenderer>().sprite);
                        QuestUI.instance.MakeProgressQuestList();
                    }
                    
                    EventManager.Notify(new SignalToNpc(q.questId, q.accomplishment, q.questInCharge));
                }
            }
        }
        
        public void QuestAccomplishment(SignalToNpc e)
        {
            var isQuestComplete = e.accomplishment;
            var questInCharge = e.questInCharge;

            var iconList = new List<QuestCompleteIcon>();
            bool findQuest = false;

            if (isQuestComplete == false)
            {
                for (int i = 0; i < questInCharge.transform.childCount; i++)
                {
                    findQuest = true;
                    
                    var icon = questInCharge.transform.GetChild(i).GetComponent<QuestCompleteIcon>();

                    if (icon) iconList.Add(icon);
                    
                    if (!icon) continue;
                    if (icon.questId == e.questId)
                    {
                        icon.InactiveQuestComplete();
                        
                        var dialogueTrigger = questInCharge.GetComponent<DialogueTrigger>();
                        DialogueManager.instance.accomplishment.Invoke(dialogueTrigger, e.questId, false);

                        iconList.Remove(icon);
                    }
                }
                
                if (!findQuest || iconList.Count == 0) return;

                foreach (var icon in iconList)
                {
                    ProcessQuest quest;
                    
                    try
                    {
                        quest = FindQuestById(icon.questId);
                        
                        if (quest.accomplishment)
                        {
                            icon.ActiveQuestComplete();
                    
                            var dialogueTrigger = questInCharge.GetComponent<DialogueTrigger>();
                            DialogueManager.instance.accomplishment.Invoke(dialogueTrigger, icon.questId, true);
                            return ;   
                        }
                    }
                    catch (NullReferenceException exception)
                    {
                        Console.WriteLine(exception);
                        
                        return;
                    }
                }
            }
            else
            {
                for (int i = 0; i < questInCharge.transform.childCount; i++)
                {
                    var icon = questInCharge.transform.GetChild(i).GetComponent<QuestCompleteIcon>();
                    
                    if (icon) iconList.Add(icon);

                    if (!icon) continue;
                    if (icon.questId == e.questId)
                    {
                        findQuest = true;
                        
                        icon.ActiveQuestComplete();
                        
                        var dialogueTrigger = questInCharge.GetComponent<DialogueTrigger>();
                        DialogueManager.instance.accomplishment.Invoke(dialogueTrigger, e.questId, true);
                        
                        iconList.Remove(icon);
                    }
                }

                if (!findQuest || iconList.Count == 0) return;

                foreach (var icon in iconList)
                {
                    
                    Debug.Log(icon);
                    icon.InactiveQuestComplete();
                }
            }
        }
        
        public void CompleteQuest(DialogueTrigger questInCharge, int id)
        {
            for (int i = 0; i < questInCharge.transform.childCount; i++)
            {
                var icon = questInCharge.transform.GetChild(i).GetComponent<QuestCompleteIcon>();
                if (!icon) continue;
                if (icon.questId == id) Destroy(icon.gameObject);
            }

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
