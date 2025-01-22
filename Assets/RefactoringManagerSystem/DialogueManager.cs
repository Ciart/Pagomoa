using System;
using System.Collections.Generic;
using System.Reflection;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Time;
using Ink.Runtime;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class DialogueManager : Manager<DialogueManager>
    {
        ~DialogueManager()
        {
            EventSystem.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        public Story story;
        
        public EntityDialogue nowEntityDialogue;
        
        public static event Action<Story> onCreateStory;
        
        private DialogueUI _dialogueUI;
        
        Dictionary<string, IDialogueCommand> _commands = new ();

        private void RegisterCommands()
        {
            foreach(var assembly in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!typeof(IDialogueCommand).IsAssignableFrom(assembly) || assembly.IsInterface) continue;
                
                var command = Activator.CreateInstance(assembly) as IDialogueCommand;
                Debug.Log(assembly.Name.Replace("DialogueCommand", "").ToLower());
                _commands.Add(assembly.Name.Replace("DialogueCommand", "").ToLower(), command);
            }
        }
        
        public bool ExecuteCommand(string command)
        {
            if (_commands.ContainsKey(command.ToLower()))
            {
                _commands[command].Execute();
                return true;
            }
            
            return false;
        }

        public override void Start()
        {
            RegisterCommands();
            
            _dialogueUI = UIManager.instance.GetUIContainer().dialogueUI;
            if (_dialogueUI == null)
            {
                Debug.LogWarning("DialogueManager::StartStory(): Could not find UI container");
            }
            
            EventSystem.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
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
            EventSystem.Notify(new StoryStarted());
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
            EventSystem.Notify(new QuestStoryStarted(quests));
        }
    }
}
