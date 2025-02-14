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
            var entityId = _entityController.entityId;
            
            Game.Instance.Quest.RegistrationQuest(portrait, entityId, id);
        }

        public void QuestComplete(string id)
        {
            Game.Instance.Quest.CompleteQuest(id);

            var questEvent = _entityController.GetComponent<IQuestEvent>();

            questEvent?.CompleteEvent();
            
            _questCompleteUI.SetActive(false);
        }

        public void StartDialogue()
        {
            var questManager = Game.Instance.Quest;
            var dialogueManager = Game.Instance.Dialogue;
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
            
            EventManager.AddListener<QuestCompleted>(OnCompleteQuestsUpdated);
            
            _entityController = GetComponent<EntityController>();
            if (_entityController == null) return;
            var entityId = _entityController.entityId;
            
            _entityQuests = ResourceSystem.instance.GetQuests(entityId);
            Debug.Log(_entityQuests.Length);

            if (_entityQuests == Array.Empty<QuestData>()) return; 
            
            var hasCompletedQuest = Game.Instance.Quest.FindCompletedQuest(_entityQuests);
            if (hasCompletedQuest)
            {
                _questCompleteUI.SetActive(true);
            }
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<QuestCompleted>(OnCompleteQuestsUpdated);
        }
    }
}