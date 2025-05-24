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
        public TextAsset basicDialogue;

        public DailyDialogue dailyDialogues;

        public Sprite portrait;
        private EntityController _entityController;
        private QuestData[] _entityQuests = Array.Empty<QuestData>();

        public Vector3 uiOffset = new Vector3(0f, 1.5f, 0f);
        private QuestCompleteIcon _questCompleteUI;

        private bool _hasInit;

        private void Start()
        {
            _questCompleteUI = Game.Instance.UI.CreateQuestCompleteUI(transform);
            _questCompleteUI.transform.position += uiOffset;

            _hasInit = true;
            OnEnable();
        }

        private void UpdateQuestIconUI()
        {
            var questManager = Game.Instance.Quest;

            var hasAvailableQuest = false;
            var hasCompletedQuest = false;

            foreach (var entityQuest in _entityQuests)
            {
                if (questManager.CheckQuestValidation(entityQuest))
                {
                    hasAvailableQuest = true;
                    continue;
                }

                var quest = questManager.FindQuestById(entityQuest.id);

                if (quest == null) continue;

                if (quest.state == QuestState.Completed)
                {
                    hasCompletedQuest = true;
                }
            }


            if (hasCompletedQuest)
            {
                _questCompleteUI.ActiveCompletableIcon();
                return;
            }
            else if (hasAvailableQuest)
            {
                _questCompleteUI.ActiveAvailableIcon();
                return;
            }

            _questCompleteUI.InactiveIcon();
        }

        public QuestData[] GetValidationQuests()
        {
            var questManager = Game.Instance.Quest;
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

            Game.Instance.Quest.RegistrationQuest(entityId, id);
        }

        public void QuestComplete(string id)
        {
            Game.Instance.Quest.CompleteQuest(id);

            var questEvent = _entityController.GetComponent<IQuestEvent>();

            questEvent?.CompleteEvent(id);

            UpdateQuestIconUI();
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
            UpdateQuestIconUI();
        }

        private void OnEnable()
        {
            if (!_hasInit) return;

            EventManager.AddListener<QuestCompleted>(OnCompleteQuestsUpdated);

            _entityController = GetComponent<EntityController>();
            if (_entityController == null) return;
            var entityId = _entityController.entityId;

            var quests = ResourceSystem.Instance.GetQuests(entityId);
            if (quests == null) return;
            _entityQuests = quests;
#if UNITY_EDITOR
            DebugCheck();
#endif          
            UpdateQuestIconUI();
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<QuestCompleted>(OnCompleteQuestsUpdated);
        }
#if UNITY_EDITOR
        private void DebugCheck()
        {
            foreach (var quest in _entityQuests)
            {
                if (quest.id == "")
                    throw new Exception($"Quest name of {quest.name}'s quest ID is empty. Check Please.");
                if (quest.title == "")
                    throw new Exception($"Quest name of {quest.name}'s title is empty. Check Please.");
                if (quest.description == "")
                    throw new Exception($"Quest name of {quest.name}'s description is empty. Check Please.");
                if (!quest.startPrologue)
                    throw new Exception($"Quest name of {quest.name}'s quest start dialogue is null. Check Please.");
                if (!quest.completePrologue)
                    throw new Exception($"Quest name of {quest.name}'s quest complete Dialogue is null. Check Please.");
                if (quest.reward.gold <= 0)
                    throw new Exception($"Quest name of {quest.name}'s reward gold is under 0. Is it right?");
                if (quest.reward.itemID == "")
                    throw new Exception($"Quest name of {quest.name}'s reward item ID is empty. Is it right?");
                if (!quest.reward.itemSprite)
                    throw new Exception($"Quest name of {quest.name}'s reward item sprite is null. Is it right?");
                if (quest.reward.value <= 0)
                    throw new Exception($"Quest name of {quest.name}'s reward item count id under 0. Is it right?");
                foreach (var condition in quest.questList)
                {
                    if (condition.summary == "")
                        throw new Exception($"Quest name of {quest.name} summary is empty that in {condition.questType}");
                    if (condition.value == "")
                        throw new Exception($"Quest name of {quest.name} value is empty that in {condition.questType}");
                    if (condition.targetID == "")
                        throw new Exception($"Quest name of {quest.name} target ID is empty that in {condition.questType}");
                }
            }
        }
#endif
    }
}
