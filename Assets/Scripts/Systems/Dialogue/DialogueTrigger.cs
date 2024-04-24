using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private TextAsset basicDialogue = null;

    [SerializeField]
    private TextAsset[] dialogueAssets;
    [SerializeField]
    private TextAsset[] questAssets;

    public void StartStory()
    {
        if (basicDialogue == null) return;
        DialogueManager.instance.SetDialogueTrigger(this);
        DialogueManager.instance.SetJsonAsset(basicDialogue);
        DialogueManager.instance.StartStory();
    }

    public void StartRandomDialogue()
    {
        var n = Random.Range(0, dialogueAssets.Length);
        DialogueManager.instance.SetJsonAsset(dialogueAssets[n]);
        DialogueManager.instance.StartStory();
    }

    public void StartQuestDialogue()
    {
        DialogueManager.instance.SetJsonAsset(questAssets[0]);
        DialogueManager.instance.StartStory();
    }
}
