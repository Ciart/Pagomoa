using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Time;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class DialogueManager : MonoBehaviour
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

        public Story story;

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

        public void StartStory(TextAsset asset)
        {
            StartStory(nowEntityDialogue, asset);
        }

        public void StartStory(EntityDialogue dialogue, TextAsset asset)
        {
            nowEntityDialogue = dialogue;
            story = new Story(asset.text);
            if (onCreateStory != null) onCreateStory(story);

            UIManager.instance.dialogueUI.gameObject.SetActive(true);
            EventManager.Notify(new StoryStarted());
            TimeManager.instance.PauseTime();
        }

        public void StopStory()
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
