using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Logger.ProcessScripts;
using UnityEngine;

namespace Ciart.Pagomoa.UI.Book
{
    public class QuestUI : MonoBehaviour
    {
        private const int MinimumQuestListSize = 8;

        public GameObject questListParent;

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
            if (_questListItems.Count < count)
            {
                for (var i = _questListItems.Count; i < count; i++)
                {
                    var item = Instantiate(questListItemUIPrefab, questListParent.transform);
                    item.onClick += id =>
                    {
                        selectQuestId = id;
                    };
                    
                    _questListItems.Add(item);
                }
            }
            else if (_questListItems.Count > count)
            {
                for (var i = _questListItems.Count - 1; i >= count; i--)
                {
                    var item = _questListItems[i];
                    _questListItems.RemoveAt(i);

                    Destroy(item.gameObject);
                }
            }
        }

        private void UpdateSelectedQuest()
        {
            foreach (var item in _questListItems)
            {
                item.isSelected = item.questId == selectQuestId;
            }
        }

        private void UpdateQuestList(List<ProcessQuest> questList)
        {
            ResizeQuestList(Math.Max(questList.Count, MinimumQuestListSize));

            for (var i = 0; i < _questListItems.Count; i++)
            {
                if (i >= questList.Count)
                {
                    _questListItems[i].UpdateUI(null);
                    continue;
                }

                _questListItems[i].UpdateUI(questList[i]);
            }

            UpdateSelectedQuest();
        }

        private void OnQuestListUpdated(QuestListUpdated e)
        {
            UpdateQuestList(e.questList);
        }

        private void OnEnable()
        {
            UpdateQuestList(QuestManager.instance.progressQuests);

            EventManager.AddListener<QuestListUpdated>(OnQuestListUpdated);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<QuestListUpdated>(OnQuestListUpdated);
        }

        private void Awake()
        {
            ResizeQuestList(MinimumQuestListSize);
        }
    }
}