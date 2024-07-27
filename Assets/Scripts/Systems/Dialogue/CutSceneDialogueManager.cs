using System;
using System.Collections;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ink.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ciart.Pagomoa
{
    public class CutSceneDialogueManager : DialogueManager
    {
        public GameObject dialogueUIPrefab;
        private GameObject _dialogueUI;
        public DialogueUI dialogueUI;

        private bool _finishInit;

        private void Start()
        {
            _dialogueUI = Instantiate(dialogueUIPrefab, transform);
            _dialogueUI.SetActive(false);
            dialogueUI = _dialogueUI.GetComponent<DialogueUI>();
        }
        
        public void StartCutSceneStory(TextAsset asset)
        {
            story = new Story(asset.text);
            //if (onCreateStory != null) onCreateStory(story);

            dialogueUI.gameObject.SetActive(true);
            EventManager.Notify(new StoryStarted());
        }
    }
}
