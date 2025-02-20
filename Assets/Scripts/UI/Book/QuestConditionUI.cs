using Ciart.Pagomoa.Logger.ProcessScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Book
{
    public class QuestConditionUI : UIBehaviour
    {
        public TextMeshProUGUI summaryText;
        
        public TextMeshProUGUI valueText;

        private RectTransform _rectTransform;

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
        }
        
        public void UpdateUI(IQuestElements condition)
        {
            summaryText.text = condition.GetQuestSummary();
            valueText.text = $"({condition.GetCompareValueToString()} / {condition.GetValueToString()})";
            


            // 높이를 텍스트 높이에 맞춤
            float totalHeight = summaryText.preferredHeight;
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, totalHeight); 
        }
    }
}