using System;
using System.Collections;
using Ciart.Pagomoa.CutScenes;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ink.Runtime;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class CutSceneDialogueManager : DialogueManager
    {
        public GameObject dialogueUIPrefab;
        private GameObject _dialogueUI;
        public DialogueUI dialogueUI;

        private Intro _intro; 

        private void Start()
        {
            _dialogueUI = Instantiate(dialogueUIPrefab, transform);
            _dialogueUI.SetActive(false);
            dialogueUI = _dialogueUI.GetComponent<DialogueUI>();

            _intro = gameObject.GetComponent<Intro>();
        }
        
        public void StartCutSceneStory(TextAsset asset)
        {
            story = new Story(asset.text);
            //if (onCreateStory != null) onCreateStory(story);

            dialogueUI.gameObject.SetActive(true);
            EventManager.Notify(new StoryStarted());
        }

        public void StopStoryAndPlayIntro()
        {
            story = null;
            _dialogueUI.SetActive(false);
            
            _intro.PlayIntro();
        }
    }
}
