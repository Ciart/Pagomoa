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
        
        public TextMeshProUGUI questRewardText;
        public GameObject wellDoneStamp;
        
        private List<QuestConditionUI> _questConditionItems = new();
        
        public void UpdateUI(Quest? quest)
        {
            if (quest is null)
            {
                wellDoneStamp.SetActive(false);
                titleText.text = "";
                descriptionText.text = "";
                questRewardText.text = "";
                npcImage.sprite = null;
                
                foreach (var item in _questConditionItems)
                {
                    Destroy(item.gameObject);
                }
                
                return;
            }
            
            titleText.text = quest.title;
            descriptionText.text = quest.description;
            npcImage.sprite = quest.npcSprite;
            questRewardText.text =
                $"보상\n" +
                $"골드 X {quest.reward.gold}\n" + 
                $"{quest.reward.itemID} X {quest.reward.value}";
            
            wellDoneStamp.SetActive(quest.state == QuestState.Finish);
            
            PrefabUtility.ResizeParentList(_questConditionItems, questConditionParent, questConditionUIPrefab, quest.conditions.Count);
            
            for (var i = 0; i < _questConditionItems.Count; i++)
            {
                _questConditionItems[i].UpdateUI(quest.conditions[i]);
            }
        }
    }
}
