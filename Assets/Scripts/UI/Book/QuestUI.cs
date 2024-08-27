using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using UnityEngine;

namespace Ciart.Pagomoa.UI.Book
{
    public class QuestUI : MonoBehaviour
    {
        private const int MinimumQuestListSize = 8;
        
        public GameObject questListParent;
        
        public QuestListItemUI questListItemUIPrefab;
        
        private List<QuestListItemUI> _questListItems;
        
        private void ResizeQuestList(int count)
        {
            if (_questListItems.Count < count)
            {
                for (var i = _questListItems.Count; i < count; i++)
                {
                    _questListItems.Add(Instantiate(questListItemUIPrefab, questListParent.transform));
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
        
        private void OnQuestListUpdated(QuestListUpdated e)
        {
            ResizeQuestList(Math.Max(e.questList.Count, MinimumQuestListSize));
            
            for (var i = 0; i < e.questList.Count; i++)
            {
                _questListItems[i].UpdateUI(e.questList[i]);
            }
        }
        
        private void OnEnable()
        {
            EventManager.AddListener<QuestListUpdated>(OnQuestListUpdated);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<QuestListUpdated>(OnQuestListUpdated);
        }
    }
}