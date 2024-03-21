using Ciart.Pagomoa.Logger.ProcessScripts;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class AssignQuestData : MonoBehaviour
    {
        public ProcessQuest assignProgressQuest;
        public void ClickToQuest()
        {
            QuestUI.instance.CheckGold(gameObject);
            QuestUI.instance.CheckTargetEntity(gameObject);
            QuestUI.instance.BasicQuest(gameObject);
            QuestUI.instance.MakeQuestValueBox(gameObject);
        }
    }
}
