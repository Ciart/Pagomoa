using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager _instance;

        private TextAsset _inkJsonAsset;

        private EntityDialogue _nowEntityDialogue;

        private enum UISelectMode
        {
            In,
            Out
        }

        private UISelectMode _uiMode = UISelectMode.Out;

        private bool _changeDialogue;

        private bool _questValid;

        [SerializeField] private GameObject outButtonGroup;

        [SerializeField] private GameObject inButtonGroup;

        [SerializeField] private Button outButtonPrefab;

        [SerializeField] private Button inButtonPrefab;

        public GameObject talkPanel;

        public Image talkImage;

        public TextMeshProUGUI talkText;

        public TextMeshProUGUI nameText;

        public List<Sprite> spriteGroup;

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

        private void Start()
        {
            EventManager.AddListener<ValidationResult>(ReturnQuestValidation);
            EventManager.AddListener<SetCompleteChat>(SetQuestCompleteChat);
        }
        
        private void Update()
        {
            SetBtnSizeAfterContentSizeFitter();
        }

        private void SetBtnSizeAfterContentSizeFitter()
        {
            if (_uiMode == UISelectMode.Out)
            {
                var rect = outButtonGroup.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector3(162f - rect.sizeDelta.x * 0.5f, 47f + rect.sizeDelta.y * 0.5f);
            }
        }

        private void RefreshView()
        {
            RemoveChildren(outButtonGroup);
            RemoveChildren(inButtonGroup);
            talkText.text = "";
            // Read all the content until we can't continue any more
            string text = "";
            while (story.canContinue)
            {
                text += story.Continue();
                text = text.Trim();
                
                if(text != "") text += "\n";
                
                ParseTag();
            }
            if(text != "") CreateContentView(text);

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
                    button.onClick.AddListener(delegate {
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
                    StopStory();
                });
            }
        }

        // When we click the choice button, tell the story to choose that choice!
        private void OnClickChoiceButton(Choice choice)
        {
            story.ChooseChoiceIndex(choice.index);
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
            var choice = Instantiate(_uiMode == UISelectMode.Out ? outButtonPrefab : inButtonPrefab);

            TextMeshProUGUI[] choiceText = choice.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var chosenText in choiceText)
                chosenText.text = text;
            
            choice.transform.SetParent(_uiMode == UISelectMode.Out ? outButtonGroup.transform : inButtonGroup.transform, false);

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

        private void ParseTag()
        {
            List<string> currentTags = story.currentTags;
            
            foreach (var currentTag in currentTags)
            {
                string prefix = currentTag.Split(' ')[0];
                string param = currentTag.Split(' ')[1];

                switch (prefix.ToLower())
                {
                    case "talker":
                        SetTalkerName(param);
                        break;
                    case "sprite":
                        SetSpriteImage(param);
                        break;
                    case "uimode":
                        if (param == "In") _uiMode = UISelectMode.In;
                        if (param == "Out") _uiMode = UISelectMode.Out;
                        break;
                    case "start":
                        if (param == "dialogue")
                        {
                            StartDailyChat();
                            _changeDialogue = true;
                        }
                        if (param == "quest")
                        {
                            StartQuestChat();
                            _changeDialogue = true;
                        }
                        break;
                    case "quest":
                        QuestAccept(param);
                        break;
                    case "reward":
                        QuestComplete(param);
                        break;

                }
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

        private void SetJsonAsset(TextAsset asset)
        {
            _inkJsonAsset = asset;
        }

        private void StartStory()
        {
            story = new Story(_inkJsonAsset.text);
            if (onCreateStory != null) onCreateStory(story);
            RefreshView();
            talkPanel.SetActive(true);
        }

        private void StopStory()
        {
            story = null;
            talkPanel.SetActive(false);
        }

        private void StartDailyChat()
        {
            SetJsonAsset(_nowEntityDialogue.dailyDialogues.GetRandomDialogue());
            StartStory();
        }

        private void StartQuestChat()
        {
            _uiMode = UISelectMode.In;
            var quests = _nowEntityDialogue.entityQuests;
            
            foreach (var quest in quests)
            {
                EventManager.Notify(new QuestValidation(quest));
                
                if (!_questValid) continue;
                
                Button button = CreateChoiceView(quest.title);
                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate
                {
                    SetJsonAsset(quest.startPrologue);
                    StartStory();
                });
            }
            
            if (quests.Length < 1)
            {
                _uiMode = UISelectMode.Out;
                CreateContentView("더이상 진행 가능한 퀘스트가 없습니다.");
                Button choice = CreateChoiceView("확인");
                choice.onClick.AddListener(delegate
                {
                    StopStory();
                });
            }
        }

        public void StartDialogue(EntityDialogue dialogue)
        {
            var icon = dialogue.transform.GetComponentInChildren<QuestCompleteIcon>();
            if (icon) {
                StartStory();
                return; }

            _nowEntityDialogue = dialogue;
            SetJsonAsset(_nowEntityDialogue.basicDialogue);
            StartStory();
        }

        private void QuestAccept(string id)
        {
            var questID = int.Parse(id);
            var interact = _nowEntityDialogue.GetComponent<InteractableObject>();
            
            Debug.Log("Quest Accept : " + questID);
                        
            EventManager.Notify(new QuestRegister(interact, questID));
        }

        private void QuestComplete(string id)
        {
            var questID = int.Parse(id);
            var interact = _nowEntityDialogue.GetComponent<InteractableObject>();
            
            Debug.Log("Quest Complete : " + questID);
            
            EventManager.Notify(new CompleteQuest(interact, questID));
        }

        private void ReturnQuestValidation(ValidationResult e)
        {
            _questValid = e.result;
        }

        private void SetQuestCompleteChat(SetCompleteChat e)
        {
            foreach (var questDialogue in _nowEntityDialogue.entityQuests)
            {
                if (e.id != questDialogue.id) continue;
                
                SetJsonAsset(questDialogue.completePrologue);
            }
        }
    }
}
