using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private TextAsset dialogueAsset = null;

    public void StartStory()
    {
        if (dialogueAsset == null) return;
        Debug.Log("시작");
        DialogueManager.Instance.SetJsonAsset(dialogueAsset);
        DialogueManager.Instance.StartStory();
    }
}
