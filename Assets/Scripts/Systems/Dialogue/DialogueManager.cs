using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Time;
using Ink.Runtime;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class DialogueManager : DialogueManagement
    {
        private static DialogueManager _instance;

        public EntityDialogue nowEntityDialogue;

        public static DialogueManager instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
                }
                return _instance;
            }
        }

        public static event Action<Story> onCreateStory;

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e) 
        {

            var player = e.player;
            var playerInput = player.GetComponent<PlayerInput>();

            playerInput.Actions.Menu.performed += context => { StopStory(); };
        }

        public override void StartStory(TextAsset asset)
        {
            StartStory(nowEntityDialogue, asset);
        }

        public override void StartStory(EntityDialogue dialogue, TextAsset asset)
        {
            nowEntityDialogue = dialogue;
            story = new Story(asset.text);
            if (onCreateStory != null) onCreateStory(story);

            UIManager.instance.dialogueUI.gameObject.SetActive(true);
            EventManager.Notify(new StoryStarted());
            TimeManager.instance.PauseTime();
        }

        public override void StopStory()
        {
            story = null;
            UIManager.instance.dialogueUI.gameObject.SetActive(false);
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
