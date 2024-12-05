using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Time;
using Ink.Runtime;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class DialogueManager : PManager<DialogueManager>
    {
        ~DialogueManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        public Story story;
        
        public EntityDialogue nowEntityDialogue;
        
        public static event Action<Story> onCreateStory;
        
        private DialogueUI _dialogueUI;

        public override void Start()
        {
            _dialogueUI = UIManager.instance.GetUIContainer().dialogueUI;
            if (_dialogueUI == null)
            {
                Debug.LogWarning("DialogueManager::StartStory(): Could not find UI container");
            }
            
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        private void OnPlayerSpawned(PlayerSpawnedEvent e) 
        {
            var player = e.player;
            var playerInput = player.GetComponent<PlayerInput>();

            playerInput.Actions.Menu.performed += context => { StopStory(); };
        }

        public void StartStory(TextAsset asset)
        {
            StartStory(nowEntityDialogue, asset);
        }

        public void StartStory(EntityDialogue dialogue, TextAsset asset)
        {
            nowEntityDialogue = dialogue;
            story = new Story(asset.text);
            if (onCreateStory != null) onCreateStory(story);
                
            _dialogueUI.gameObject.SetActive(true);
            EventManager.Notify(new StoryStarted());
            TimeManager.instance.PauseTime();
        }

        public void StopStory()
        {
            story = null;
            _dialogueUI.gameObject.SetActive(false);
            TimeManager.instance.ResumeTime();
        }

        public void StartDailyChat()
        {
            StartStory(nowEntityDialogue, nowEntityDialogue.dailyDialogues.GetRandomDialogue());
        }

        public void StartQuestChat()
        {
            var quests = nowEntityDialogue.GetValidationQuests();
            EventManager.Notify(new QuestStoryStarted(quests));
        }
    }
}
