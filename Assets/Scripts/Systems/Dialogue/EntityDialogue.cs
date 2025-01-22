using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.RefactoringManagerSystem;
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
        
        private bool _hasInit; 
        
        private void Start()
        {
            _questCompleteUI = Game.Instance.UI.CreateQuestCompleteUI(transform);
            _questCompleteUI.SetActive(false);
            _questCompleteUI.transform.position += uiOffset;
            
            _hasInit = true;
            OnEnable();
        }
        
        public QuestData[] GetValidationQuests()
        { 
            var questManager = QuestManager.instance; 
            var result = new List<QuestData>();

            foreach (var quest in _entityQuests)
            {
                if (questManager.CheckQuestValidation(quest))
                { 
                    result.Add(quest);
                }
            }

            return result.ToArray();
        }

        public void QuestAccept(string id)
        {
            var questManager = QuestManager.instance;

            var entityId = _entityController.entityId;
            
            questManager.RegistrationQuest(portrait, entityId, id);
        }

        public void QuestComplete(string id)
        {
            var questManager = QuestManager.instance;

            questManager.CompleteQuest(id);

            var questEvent = _entityController.GetComponent<IQuestEvent>();

            questEvent?.CompleteEvent();
            
            _questCompleteUI.SetActive(false);
        }

        public void StartDialogue()
        {
            Debug.Log(_entityQuests.Length);
            var questManager = QuestManager.instance;
            var dialogueManager = DialogueManager.instance;
            var icon = transform.GetComponentInChildren<QuestCompleteIcon>();

            if (icon)
            {
                foreach (var quest in _entityQuests)
                {
                    var progressQuest = questManager.FindQuestById(quest.id);
                    
                    if (progressQuest is null || progressQuest.state != QuestState.Completed)
                    {
                        continue;
                    }
                    
                    dialogueManager.StartStory(this, quest.completePrologue);
                    return;
                }
            }
            
            dialogueManager.StartStory(this, basicDialogue);
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
            if(!_hasInit) return;
         
            var questManager = QuestManager.instance;
            
            EventSystem.AddListener<QuestCompleted>(OnCompleteQuestsUpdated);
            
            _entityController = GetComponent<EntityController>();
            if (_entityController == null) return;
            var entityId = _entityController.entityId;
            
            if (questManager is null) return;
            
            _entityQuests = questManager.database.GetEntityQuestsByEntity(entityId);

            if (_entityQuests == Array.Empty<QuestData>()) return; 
            
            var hasCompletedQuest = questManager.FindCompletedQuest(_entityQuests);
            if (hasCompletedQuest)
            {
                _questCompleteUI.SetActive(true);
            }
        }

        private void OnDisable()
        {
            EventSystem.RemoveListener<QuestCompleted>(OnCompleteQuestsUpdated);
        }
    }
}