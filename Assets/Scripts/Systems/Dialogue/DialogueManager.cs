using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
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

        public void StartStory(TextAsset asset)
        {
            StartStory(nowEntityDialogue, asset);
        }

        public void StartStory(EntityDialogue dialogue, TextAsset asset)
        {
            nowEntityDialogue = dialogue;
            story = new Story(asset.text);
            if (onCreateStory != null) onCreateStory(story);

            EventManager.Notify(new UISetVisible(true));
            EventManager.Notify(new RefreshView());
        }

        public void StopStory()
        {
            story = null;
            EventManager.Notify(new UISetVisible(false));
        }

        public void StartDailyChat()
        {
            StartStory(nowEntityDialogue, nowEntityDialogue.dailyDialogues.GetRandomDialogue());
        }

        public void StartQuestChat()
        {
            EventManager.Notify(new UIMode(DialogueUI.UISelectMode.In));

            var quests = nowEntityDialogue.GetValidationQuests();
            EventManager.Notify(new MakeQuestContentView(quests));
        }


    }
}
