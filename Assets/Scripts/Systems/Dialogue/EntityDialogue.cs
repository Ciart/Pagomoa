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
            if (Game.Get<UIManager>() == null) return;
            
            _questCompleteUI = Game.Get<UIManager>().CreateQuestCompleteUI(transform);
            _questCompleteUI.SetActive(false);
            _questCompleteUI.transform.position += uiOffset;
            
            _hasInit = true;
            OnEnable();
        }
        
        public QuestData[] GetValidationQuests()
        {
           var result = new List<QuestData>();

           foreach (var quest in _entityQuests)
           {
               if (Game.Get<QuestManager>().CheckQuestValidation(quest))
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
            
            Game.Get<QuestManager>().RegistrationQuest(portrait, origin, id);
        }

        public void QuestComplete(string id)
        {
            Debug.Log("Quest Complete : " + id);

            Game.Get<QuestManager>().CompleteQuest(id);
            
            _questCompleteUI.SetActive(false);
        }

        public void StartDialogue()
        {
            var icon = transform.GetComponentInChildren<QuestCompleteIcon>();

            if (icon)
            {
                foreach (var quest in _entityQuests)
                {
                    var progressQuest = Game.Get<QuestManager>().FindQuestById(quest.id);
                    
                    if (progressQuest is null || progressQuest.state != QuestState.Completed)
                    {
                        continue;
                    }
                    
                    Game.Get<DialogueManager>().StartStory(this, quest.completePrologue);
                    return;
                }
            }

            Game.Get<DialogueManager>().StartStory(this, basicDialogue);
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
            var origin = _entityController.origin;
            
            if (Game.Get<QuestManager>() is null) return;
            
            _entityQuests = Game.Get<QuestManager>().database.GetEntityQuestsByEntity(origin);

            if (_entityQuests == Array.Empty<QuestData>()) return; 
            
            var hasCompletedQuest = Game.Get<QuestManager>().FindCompletedQuest(_entityQuests);
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