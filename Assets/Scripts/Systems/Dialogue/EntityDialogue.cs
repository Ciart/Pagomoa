using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Logger.ProcessScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class EntityDialogue : MonoBehaviour
    {
        public TextAsset basicDialogue = null;

        public DailyDialogue dailyDialogues;

        public Sprite portrait;
        private EntityController _entityController;
        private QuestData[] _entityQuests;

        public Vector3 uiOffset = new Vector3(0f, 3.2f, 0f);
        private GameObject _questCompleteUI;
        
        private void Start()
        { 
            if (!UIManager.instance) return;
            
            _questCompleteUI = UIManager.CreateQuestCompleteUI(transform);
            _questCompleteUI.SetActive(false);
            _questCompleteUI.transform.position += uiOffset;
        }
        
        public QuestData[] GetValidationQuests()
        {
           var result = new List<QuestData>();

           foreach (var quest in _entityQuests)
           {
               if (QuestManager.instance.CheckQuestValidation(quest))
               {
                   result.Add(quest);
               }
           }

           return result.ToArray();
        }

        public void QuestAccept(string id)
        {
            Debug.Log("Quest Accept : " + id);

            var origin = _entityController.origin;
            
            QuestManager.instance.RegistrationQuest(portrait, origin, id);
        }

        public void QuestComplete(string id)
        {
            Debug.Log("Quest Complete : " + id);

            QuestManager.instance.CompleteQuest(id);
            
            _questCompleteUI.SetActive(false);
        }

        public void StartDialogue()
        {
            var icon = transform.GetComponentInChildren<QuestCompleteIcon>();

            if (icon)
            {
                foreach (var quest in _entityQuests)
                {
                    var progressQuest = QuestManager.instance.FindQuestById(quest.id);
                    
                    if (progressQuest is null || progressQuest.state != QuestState.Completed)
                    {
                        continue;
                    }
                    
                    DialogueManager.instance.StartStory(this, quest.completePrologue);
                    return;
                }
            }

            DialogueManager.instance.StartStory(this, basicDialogue);
        }

        private void OnCompleteQuestsUpdated(QuestCompleted e)
        {
            foreach (var entityQuest in _entityQuests)
            {
                if (e.quest.id == entityQuest.id)
                {
                    _questCompleteUI.SetActive(true);
                    return;
                }
            }
            
            _questCompleteUI.SetActive(false);
        }

        private void OnEnable()
        {
            _entityController = GetComponent<EntityController>();
            if (_entityController == null) return;
            var origin = _entityController.origin;
            
            if (!QuestManager.instance) return;
            
            _entityQuests = QuestManager.instance.database.GetEntityQuestsByEntityID(origin);

            if (_entityQuests == Array.Empty<QuestData>()) return; 
            
            var hasCompletedQuest = QuestManager.instance.FindCompletedQuest(_entityQuests);
            if (hasCompletedQuest)
            {
                _questCompleteUI.SetActive(true);
            }
            
            EventManager.AddListener<QuestCompleted>(OnCompleteQuestsUpdated);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<QuestCompleted>(OnCompleteQuestsUpdated);
        }
    }
}