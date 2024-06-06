using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Logger;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {

        private static DialogueManager _instance = null;

        private TextAsset _inkJsonAsset = null;

        private EntityDialogue _nowEntityDialogue;

        private enum UISelectMode
        {
            In,
            Out
        }

        private UISelectMode _uiMode = UISelectMode.Out;

        private bool _changeDialogue = false;

        [SerializeField] private GameObject _outButtonGroup;

        [SerializeField] private GameObject _inButtonGroup;

        [SerializeField] private Button _outButtonPrefab = null;

        [SerializeField] private Button _inButtonPrefab = null;

        [Space]

        public GameObject talkPannel;

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

        private void Update()
        {
            SetBtnSizeAfterContentSizeFitter();
        }

        private void SetBtnSizeAfterContentSizeFitter()
        {
            if (_uiMode == UISelectMode.Out)
            {
                var rect = _outButtonGroup.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector3(162f - rect.sizeDelta.x * 0.5f, 47f + rect.sizeDelta.y * 0.5f);
            }
        }

        private void RefreshView()
        {
            RemoveChildren(_outButtonGroup);
            RemoveChildren(_inButtonGroup);
            talkText.text = "";
            // Read all the content until we can't continue any more
            string text = "";
            while (story.canContinue)
            {
                text += story.Continue();
                text = text.Trim();
                if(text != "") text += "\n";
                Debug.Log(text);
                ParseTag();
            }
            if(text != "") CreateContentView(text);

            if (_changeDialogue == true)
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
            var choice = Instantiate(_uiMode == UISelectMode.Out ? _outButtonPrefab : _inButtonPrefab);

            TextMeshProUGUI[] choiceText = choice.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var chosedtext in choiceText)
                chosedtext.text = text;


            choice.transform.SetParent(_uiMode == UISelectMode.Out ? _outButtonGroup.transform : _inButtonGroup.transform, false);


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
            List<string> tags = new List<string>();
            tags = story.currentTags;

            foreach (var tag in tags)
            {
                string prefix = tag.Split(' ')[0];
                string param = tag.Split(' ')[1];

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

        private void SetTalkerName(string name)
        {
            nameText.text = name;
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
                string name = sprite.name;
                if (name.Replace(" ", string.Empty) == param)
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
            talkPannel.SetActive(true);
        }

        private void StopStory()
        {
            story = null;
            talkPannel.SetActive(false);
        }

        private void StartDailyChat()
        {
            SetJsonAsset(_nowEntityDialogue.dailyDialogues.GetRandomDialogue());
            StartStory();
        }

        private void StartQuestChat()
        {
            _uiMode = UISelectMode.In;
            var quests = _nowEntityDialogue.questDialogues;
            foreach (var quest in quests)
            {
                if (!quest.IsPrerequisiteCompleted()) continue;
                if (QuestManager.instance.IsRegisteredQuest(quest.questId)) continue;
                if (QuestManager.instance.IsCompleteQuest(quest.questId)) continue;

                Button button = CreateChoiceView(quest.questName);
                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate
                {
                    SetJsonAsset(quest.questStartPrologos);
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

        public void StartQuestCompleteChat(int id)
        {
            foreach (var questDialogue in _nowEntityDialogue.questDialogues)
            {
                if (id == questDialogue.questId)
                {
                     SetJsonAsset(questDialogue.questCompletePrologos);
                     StartStory();
                }
            }
        }

        public void StartDialogue(EntityDialogue dialogue)
        {
            _nowEntityDialogue = dialogue;
            SetJsonAsset(_nowEntityDialogue.basicDialogue);
            StartStory();
        }

        private void QuestAccept(string id)
        {
            var questId = int.Parse(id);
            var interact = _nowEntityDialogue.GetComponent<InteractableObject>();

            if (interact is null)
            {
                Debug.LogError("is not interactable quest NPC.");
                return ;
            }
            
            Debug.Log("Quest Accept : " + questId);
            
            QuestManager.instance.registerQuest.Invoke(interact, questId);
        }

        private void QuestComplete(string id)
        {
            var questId = int.Parse(id);
            var interact = _nowEntityDialogue.GetComponent<InteractableObject>();
            
            if (interact is null)
            {
                Debug.LogError("is not interactable quest NPC.");
                return ;
            }
            
            Debug.Log("Quest Complete : " + questId);
            
            QuestManager.instance.completeQuest.Invoke(interact, questId);
        }
    }
}
