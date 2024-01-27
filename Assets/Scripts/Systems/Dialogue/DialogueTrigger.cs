using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private TextAsset dialogueAsset = null;

    public void StartStory()
    {
        if (dialogueAsset == null) return;
        Debug.Log("Ω√¿€");
        DialogueManager.Instance.SetJsonAsset(dialogueAsset);
        DialogueManager.Instance.StartStory();
    }
}
