using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Playables;

namespace Ciart.Pagomoa
{
    public class CutSceneDialogue : DialogueManagement
    {
        public GameObject dialogueUIPrefab;
        private GameObject _dialogueUI;
        public DialogueUI dialogueUI;

        private PlayableDirector _director;

        private void Start()
        {
            _dialogueUI = Instantiate(dialogueUIPrefab, transform);
            _dialogueUI.SetActive(false);
            dialogueUI = _dialogueUI.GetComponent<DialogueUI>();

            _director = gameObject.GetComponent<PlayableDirector>();
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
            
            _director.Play();
        }
    }
}
