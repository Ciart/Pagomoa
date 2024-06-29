using System.Collections.Generic;
using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class EntityDialogue : MonoBehaviour
    {
        public TextAsset basicDialogue = null;

        public DailyDialogue dailyDialogues;

        public Quest[] entityQuests;

        private void Start()
        {
            for (int i = 0; i < entityQuests.Length; i++)
            {
                QuestManager.instance.database.quests.Add(entityQuests[i]);
            }
        }
        
        public Quest[] GetValidationQuests()
        {
           var result = new List<Quest>();

           foreach (var quest in entityQuests)
           {
               if (QuestManager.instance.CheckQuestValidation(quest))
               {
                   result.Add(quest);
               }
           }

           return result.ToArray();
        }

        public void QuestAccept(string id)
        {
            var interact = GetComponent<InteractableObject>();

            Debug.Log("Quest Accept : " + id);

            QuestManager.instance.RegistrationQuest(interact, id);
        }

        public void QuestComplete(string id)
        {
            var interact = GetComponent<InteractableObject>();

            Debug.Log("Quest Complete : " + id);

            QuestManager.instance.CompleteQuest(interact, id);
        }

        public void StartDialogue()
        {
            var icon = transform.GetComponentInChildren<QuestCompleteIcon>();

            if (icon)
            {
                foreach (var quest in entityQuests)
                {
                    var progressQuest = QuestManager.instance.FindQuestById(quest.id);

                    if (!progressQuest.accomplishment)
                    {
                        continue;
                    }
                    
                    DialogueManager.instance.StartStory(this, quest.completePrologue);
                    return;
                }
            }

            DialogueManager.instance.StartStory(this, basicDialogue);
        }
    }
}