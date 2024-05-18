using Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private TextAsset basicDialogue;

        [SerializeField]
        private DailyDialogue dailyDialogues;
    
        [SerializeField]
        private Quest[] quests;

        public void StartStory()
        {
            if (basicDialogue == null) return;
            DialogueManager.instance.SetJsonAsset(basicDialogue);
            DialogueManager.instance.StartStory(this);
        }
        
        public void StartDialogue()
        {
            DialogueManager.instance.SetJsonAsset(dailyDialogues.GetRandomDialogue());
            DialogueManager.instance.StartStory(this);
        }

        public Quest[] GetQuestDialogue()
        {
            return quests;
        }

        public void QuestCompleteDialogue(int id)
        {
            foreach (var quest in quests)
            {
                if (id == quest.id)
                {
                    DialogueManager.instance.SetJsonAsset(quest.completePrologue);
                }
            }
        }
    
    }
}
