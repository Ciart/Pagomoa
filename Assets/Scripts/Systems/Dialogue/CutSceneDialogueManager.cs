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
    public class CutSceneDialogueManager : DialogueManagement
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
        
        public void StartCutSceneStory(TextAsset storyAsset)
        {
            story = new Story(storyAsset.text);

            dialogueUI.gameObject.SetActive(true);
            EventManager.Notify(new StoryStarted(this));
        }

        public override void StopStory()
        {
            story = null;
            _dialogueUI.SetActive(false);
            
            _intro.PlayIntro();
        }
    }
}
