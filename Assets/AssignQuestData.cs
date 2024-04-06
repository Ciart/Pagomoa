using Ciart.Pagomoa.Logger.ProcessScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa
{
    public class AssignQuestData : MonoBehaviour,IPointerClickHandler
    {
        public ProcessQuest assignProgressQuest;
        public void ClickToQuest()
        {
            QuestUI.instance.CheckGold(gameObject);
            QuestUI.instance.CheckTargetEntity(gameObject);
            QuestUI.instance.BasicQuest(gameObject);
            QuestUI.instance.MakeQuestValueBox(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickToQuest();
        }
    }
}
