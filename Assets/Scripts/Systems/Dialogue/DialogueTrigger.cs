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

    private bool _isQuestDialogue;
    
    public void StartStory()
    {
        if (basicDialogue == null) return;
        DialogueManager.instance.SetDialogueTrigger(this);
        DialogueManager.instance.SetJsonAsset(basicDialogue);
        DialogueManager.instance.StartStory();
    }

    public void StartSetUpTextStory()
    {
        if (basicDialogue == null) return;
        DialogueManager.instance.SetDialogueTrigger(this);
        
        if (_isQuestDialogue == false)
        {
            DialogueManager.instance.SetJsonAsset(basicDialogue);
            DialogueManager.instance.StartStory();
        }
        else
        {
            DialogueManager.instance.StartStory();
            _isQuestDialogue = false;
        }
    }
    
    public void InitDialogue()
    {
        DialogueManager.instance.SetJsonAsset(basicDialogue);
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
                DialogueManager.instance.SetJsonAsset(questDialogue.questCompletePrologue);
                _isQuestDialogue = true;
            }
        }
    }
    
}
