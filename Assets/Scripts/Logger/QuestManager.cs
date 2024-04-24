using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using Logger;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Logger
{
    public delegate void RegisterQuest(InteractableObject questInCharge, int id);

    [Serializable]
    [RequireComponent(typeof(QuestDatabase))]
    public class QuestManager : MonoBehaviour
    {
        public List<ProcessQuest> progressQuests = new List<ProcessQuest>();
        public QuestDatabase database;
        
        public RegisterQuest registerQuest;
        
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
                    
                    Debug.Log(q);
                }
            }
        }

        private void QuestAccomplishment(SignalToNpc e)
        {
            var isQuestComplete = e.accomplishment;
            var questInCharge = e.questInCharge;

            if (isQuestComplete == false)
            {
                questInCharge.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                questInCharge.transform.GetChild(1).gameObject.SetActive(true);
                questInCharge.interactionEvent.SetPersistentListenerState(0, UnityEventCallState.Off);
                questInCharge.interactionEvent.AddListener(() => GetReward(questInCharge)); // todo: 퀘스트 완료 대화
            }
        }

        private void GetReward(InteractableObject questInCharge)
        {
            questInCharge.transform.GetChild(1).gameObject.SetActive(false);
            questInCharge.interactionEvent.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
            questInCharge.interactionEvent.RemoveListener(() => GetReward(questInCharge));            
        }
    }   
}
