﻿using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Dialogue;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Ciart.Pagomoa
{
    public enum UISelectMode
    {
        In,
        Out
    }
    
    public class DialogueUI : MonoBehaviour
    {
        public GameObject outButtonGroup;

        public GameObject inButtonGroup;

        public Button outButtonPrefab;

        public Button inButtonPrefab;

        public GameObject talkPanel;

        public Image talkImage;

        public TextMeshProUGUI talkText;

        public TextMeshProUGUI nameText;

        public List<Sprite> spriteGroup;

        public UISelectMode uiMode = UISelectMode.Out;

        private DialogueUI _dialogueUI = null;

        private DialogueManager _targetManager;
        
        private bool _changeDialogue;

        private void Awake()
        {
            _dialogueUI = GetComponent<DialogueUI>();
        }

        private void OnEnable()
        {
            EventManager.AddListener<StoryStarted>(RefreshView);
            EventManager.AddListener<QuestStoryStarted>(MakeQuestContentView);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<StoryStarted>(RefreshView);
            EventManager.RemoveListener<QuestStoryStarted>(MakeQuestContentView);
        }


        private void Update()
        {
            SetBtnSizeAfterContentSizeFitter();
        }

        private void SetBtnSizeAfterContentSizeFitter()
        {
            if (uiMode == UISelectMode.Out)
            {
                var rect = _dialogueUI.outButtonGroup.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector3(162f - rect.sizeDelta.x * 0.5f, 47f + rect.sizeDelta.y * 0.5f);
            }
        }

        private void RefreshView(StoryStarted obj)
        {
            if (obj.targetManagement != null)
                _targetManager = obj.targetManagement;
            else
            {
                var dialogueManager = DialogueManager.instance;
                
                _targetManager = dialogueManager;
            }
            
            RefreshView();
        }

        private void RefreshView()
        {
            var story = _targetManager.story;

            RemoveChildren(_dialogueUI.outButtonGroup);
            RemoveChildren(_dialogueUI.inButtonGroup);
            _dialogueUI.talkText.text = "";
            // Read all the content until we can't continue any more
            string text = "";
            while (story.canContinue)
            {
                text += story.Continue();
                text = text.Trim();

                if (text != "") text += "\n";
                
                ParseTag();
            }
            if (text != "") CreateContentView(text);

            if (_changeDialogue)
            {
                _changeDialogue = false;
                return;
            }
            // Display all the choices, if there are any!
            if (story.currentChoices.Count > 0)
            {
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    Choice choice = story.currentChoices[i];
                    Button button = CreateChoiceView(choice.text.Trim());
                    // Tell the button what to do when we press it
                    button.onClick.AddListener(delegate
                    {
                        OnClickChoiceButton(choice);
                    });
                }
            }
            // If we've read all the content and there's no choices, the story is finished!
            else
            {
                Button choice = CreateChoiceView("확인");
                choice.onClick.AddListener(delegate
                {
                    _targetManager.StopStory();
                });
            }
        }

        public void ParseTag()
        {
            List<string> currentTags = _targetManager.story.currentTags;

            foreach (var currentTag in currentTags)
            {
                var dialogueManager = DialogueManager.instance;
                
                var prefix = currentTag.Split(' ')[0];
                var param = currentTag.Split(' ')[1];

                switch (prefix.ToLower())
                {
                    case "talker":
                        SetTalkerName(param);
                        break;
                    case "sprite":
                        SetSpriteImage(param);
                        break;
                    case "uimode":
                        if (param == "In") uiMode = UISelectMode.In;
                        if (param == "Out") uiMode = UISelectMode.Out;
                        break;
                    case "start":
                        if (param == "dialogue")
                        {
                            dialogueManager.StartDailyChat();
                            _changeDialogue = true;
                        }
                        else if (param == "quest")
                        {
                            dialogueManager.StartQuestChat();
                            _changeDialogue = true;
                        }
                        else
                        {
                            if (!DialogueManager.instance.ExecuteCommand(param))
                            {
                                Debug.LogError("Dialogue command not found : " + param);
                            }
                            _changeDialogue = true;
                        }
                        break;
                    case "quest":
                        dialogueManager.nowEntityDialogue.QuestAccept(param);
                        break;
                    case "reward":
                        dialogueManager.nowEntityDialogue.QuestComplete(param);
                        break;
                }
            }
        }

        // When we click the choice button, tell the story to choose that choice!
        private void OnClickChoiceButton(Choice choice)
        {
            _targetManager.story.ChooseChoiceIndex(choice.index);

            RefreshView();
        }

        // Creates a textbox showing the the line of text
        private void CreateContentView(string text)
        {
            talkText.text = text;
        }

        // Creates a button showing the choice text
        private Button CreateChoiceView(string text)
        {
            var choice = Instantiate(uiMode == UISelectMode.Out ? outButtonPrefab : inButtonPrefab);

            TextMeshProUGUI[] choiceText = choice.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var chosenText in choiceText)
                chosenText.text = text;
            choice.transform.SetParent(uiMode == UISelectMode.Out ? outButtonGroup.transform : inButtonGroup.transform, false);

            var layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;


            return choice;
        }

        // Destroys all the children of this gameobject (all the UI)
        private void RemoveChildren(GameObject vas)
        {
            int childCount = vas.transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                Destroy(vas.transform.GetChild(i).gameObject);
            }
        }

       
        private void SetTalkerName(string talkerName)
        {
            nameText.text = talkerName;
        }

        private void SetSpriteImage(string param)
        {
            if (param == "null")
            {
                talkImage.gameObject.SetActive(false);
                return;
            }

            var cutSceneController = DataBase.data.GetCutSceneController();

            if (cutSceneController.CutSceneIsPlayed())
            {
                var target = cutSceneController.GetOnPlayingCutScene();
                
                foreach (var sprite in target.GetSprites())
                {
                    if (sprite.name.Replace(" ", string.Empty) == param)
                    {
                        talkImage.sprite = sprite;
                        talkImage.gameObject.SetActive(true);
                        return;
                    }
                }
                
                return;
            }
            
            foreach (var sprite in spriteGroup)
            {
                string spriteName = sprite.name;
                if (spriteName.Replace(" ", string.Empty) == param)
                {
                    talkImage.sprite = sprite;
                    talkImage.gameObject.SetActive(true);
                    return;
                }
            }

            Debug.LogError("no image There");
        }

        private void MakeQuestContentView(QuestStoryStarted qc)
        {
            var quests = qc.quests;
            uiMode = UISelectMode.In;
            foreach (var quest in quests)
            {
                Button button = CreateChoiceView(quest.title);
                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate
                {
                    DialogueManager.instance.StartStory(quest.startPrologue);
                });
            }

            if (quests.Length < 1)
            {
                uiMode = UISelectMode.Out;

                CreateContentView("더이상 진행 가능한 퀘스트가 없습니다.");
                Button choice = CreateChoiceView("확인");
                choice.onClick.AddListener(delegate
                {
                    DialogueManager.instance.StopStory();
                });
            }
        }
    }
}
