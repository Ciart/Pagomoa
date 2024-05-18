using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Events;
using Ciart.Pagomoa;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private TextAsset basicDialogue = null;

    [SerializeField]
    private DailyDialogue dailyDialogues;
    [SerializeField]
    private QuestDialogue[] questDialogues;

    public void StartStory()
    {
        if (basicDialogue == null) return;
        DialogueManager.instance.SetDialogueTrigger(this);
        DialogueManager.instance.SetJsonAsset(basicDialogue);
        DialogueManager.instance.StartStory();
    }

    public void StartDialogue()
    {
        DialogueManager.instance.SetJsonAsset(dailyDialogues.GetRandomDialogue());
        DialogueManager.instance.StartStory();
    }

    public QuestDialogue[] GetQuestDialogue()
    {
        return questDialogues;
    }

    public void QuestCompleteDialogue(int id)
    {
        foreach (var questDialogue in questDialogues)
        {
            if (id == questDialogue.questId)
            {
                DialogueManager.instance.SetJsonAsset(questDialogue.questCompletePrologos);
                DialogueManager.instance.StartStory();
            }
        }
    }
    
}
