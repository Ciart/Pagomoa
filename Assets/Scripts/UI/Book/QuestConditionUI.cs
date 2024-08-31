using Ciart.Pagomoa.Logger.ProcessScripts;
using TMPro;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.UI.Book
{
    public class QuestConditionUI : UIBehaviour
    {
        public TextMeshProUGUI summaryText;
        
        public TextMeshProUGUI valueText;
        
        public void UpdateUI(IQuestElements condition)
        {
            summaryText.text = condition.GetQuestSummary();
            valueText.text = $"({condition.GetCompareValueToString()} / {condition.GetValueToString()})";
        }
    }
}