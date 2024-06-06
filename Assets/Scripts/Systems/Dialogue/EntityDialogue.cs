using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Events;
using Ciart.Pagomoa;

public class EntityDialogue : MonoBehaviour
{
    public TextAsset basicDialogue = null;

    public DailyDialogue dailyDialogues;

    public QuestDialogue[] questDialogues;

    public void QuestCompleteDialogue(int id)
    {
        foreach (var questDialogue in questDialogues)
        {
            if (id == questDialogue.questId)
            {
                //DialogueManager.instance.SetJsonAsset(questDialogue.questCompletePrologos);
                //DialogueManager.instance.StartStory();
            }
        }
    }
    
}
