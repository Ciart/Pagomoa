using System.Collections.Generic;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Utilities;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Book
{
    public class QuestDetailUI : UIBehaviour
    {
        public TextMeshProUGUI titleText;
        
        public TextMeshProUGUI descriptionText;
        
        public Image npcImage;
        
        public GameObject questConditionParent;
        
        public QuestConditionUI questConditionUIPrefab;
        
        private List<QuestConditionUI> _questConditionItems = new();
        
        public void UpdateUI([CanBeNull] ProcessQuest quest)
        {
            if (quest is null)
            {
                titleText.text = "";
                descriptionText.text = "";
                npcImage.sprite = null;
                
                foreach (var item in _questConditionItems)
                {
                    Destroy(item.gameObject);
                }
                
                return;
            }
            
            titleText.text = quest.title;
            descriptionText.text = quest.description;
            npcImage.sprite = quest.mNpcSprite;
            
            PrefabUtility.ResizeParentList(_questConditionItems, questConditionParent, questConditionUIPrefab, quest.elements.Count);
            
            for (var i = 0; i < _questConditionItems.Count; i++)
            {
                _questConditionItems[i].UpdateUI(quest.elements[i]);
            }
        }
    }
}
