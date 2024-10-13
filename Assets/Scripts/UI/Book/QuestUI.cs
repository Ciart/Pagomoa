using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Utilities;
using UnityEngine;

namespace Ciart.Pagomoa.UI.Book
{
    public class QuestUI : MonoBehaviour
    {
        private const int MinimumQuestListSize = 8;

        public GameObject questListParent;
        
        public QuestDetailUI questDetailUI;

        public QuestListItemUI questListItemUIPrefab;

        private string _selectQuestId;
        
        public string selectQuestId
        {
            get => _selectQuestId;
            private set
            {
                _selectQuestId = value;
                UpdateSelectedQuest();
            }
        }
        
        private List<QuestListItemUI> _questListItems = new();

        private void ResizeQuestList(int count)
        {
            PrefabUtility.ResizeParentList(_questListItems, questListParent, questListItemUIPrefab, count, item =>
            {
                item.onClick += id =>
                {
                    selectQuestId = id;
                };
            });
        }

        private void UpdateSelectedQuest()
        {
            foreach (var item in _questListItems)
            {
                item.isSelected = item.questId == selectQuestId;
            }
            
            UpdateQuestDetail(QuestManager.instance.FindQuestById(selectQuestId));
        }

        private void UpdateQuestList(List<Quest> questList)
        {
            ResizeQuestList(Math.Max(questList.Count, MinimumQuestListSize));

            for (var i = 0; i < _questListItems.Count; i++)
            {
                if (i >= questList.Count)
                {
                    _questListItems[i].UpdateUI(null);
                    continue;
                }
                
                if (i == 0 && selectQuestId == "")
                {
                    selectQuestId = questList[i].id;
                }

                _questListItems[i].UpdateUI(questList[i]);
            }

            UpdateSelectedQuest();
        }
        
        private void UpdateQuestDetail(Quest quest)
        {
            if (quest?.id != selectQuestId) return;
            
            questDetailUI.UpdateUI(quest);
        }

        private void OnQuestListUpdated(QuestListUpdated e)
        {
            UpdateQuestList(e.questList);
        }
        
        private void OnQuestUpdated(QuestUpdated e)
        {
            UpdateQuestDetail(e.quest);
        }

        private void OnEnable()
        {
            var questManager = QuestManager.instance;
            
            UpdateQuestList(questManager.quests);
            UpdateQuestDetail(questManager.FindQuestById(selectQuestId));

            EventManager.AddListener<QuestListUpdated>(OnQuestListUpdated);
            EventManager.AddListener<QuestUpdated>(OnQuestUpdated);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<QuestListUpdated>(OnQuestListUpdated);
            EventManager.RemoveListener<QuestUpdated>(OnQuestUpdated);
        }

        private void Awake()
        {
            ResizeQuestList(MinimumQuestListSize);
        }
    }
}