using Ciart.Pagomoa.Logger.ProcessScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa
{
    public class AssignQuestData : MonoBehaviour,IPointerClickHandler
    {
        public ProcessQuest assignProgressQuest;
        public Sprite npcImage;
        public void ClickToQuest()
        {
            QuestUI.instance.CheckGold(gameObject);
            QuestUI.instance.CheckTargetEntity(gameObject);
            QuestUI.instance.BasicQuest(gameObject);
            QuestUI.instance.MakeQuestValueBox(gameObject);
            QuestUI.instance.npcImageObject.sprite = npcImage;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            ClickToQuest();
        }
    }
}
