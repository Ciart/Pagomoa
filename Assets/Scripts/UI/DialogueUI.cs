using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Choice = Ink.Runtime.Choice;

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

        [SerializeField] private OutModeButton _outButtonPrefab;
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

        private Vector2 _talkImageOriginalPosition;

        private void Awake()
        {
            _dialogueUI = GetComponent<DialogueUI>();

            _talkImageOriginalPosition = talkImage.rectTransform.anchoredPosition;
        }

        private void OnEnable()
        {
            EventManager.AddListener<StoryStarted>(RefreshView);
            EventManager.AddListener<QuestStoryStarted>(MakeQuestContentView);
            Game.Instance.Time.PauseTime();
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<StoryStarted>(RefreshView);
            EventManager.RemoveListener<QuestStoryStarted>(MakeQuestContentView);
            Game.Instance.Time.ResumeTime();
        }

        private void SetBtnSizeAfterContentSizeFitter()
        {
            if (!_targetManager.story) return; // 상점과 같은 버튼이 없는 대화 출력시 버튼 에러 방지
            var buttonCount = _targetManager.story.currentChoices.Count;

            if (buttonCount == 0) buttonCount = 1; // 마지막 선택 시 때문에 설정
            _dialogueUI.outButtonGroup.TryGetComponent<RectTransform>(out var rect);
            var buttonSize = _outButtonPrefab.rectTransform.sizeDelta;
            rect.anchoredPosition = new Vector2(0f, (buttonSize.y + 4) * buttonCount);
        }

        private void RefreshView(StoryStarted obj)
        {
            if (obj.targetManagement != null)
                _targetManager = obj.targetManagement;
            else
            {
                var dialogueManager = Game.Instance.Dialogue;

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
                SetBtnSizeAfterContentSizeFitter();

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
                var dialogueManager = Game.Instance.Dialogue;

                var prefix = currentTag.Split(' ')[0];
                var param = currentTag.Split(' ')[1];

                switch (prefix.ToLower())
                {
                    case "actor":
                        SetActor(param);
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
                        else if (param == "shop")
                        {
                            Game.Instance.UI.shopUI.ActiveShop();
                            _targetManager.StopStory();
                        }
                        else
                        {
                            if (!Game.Instance.Dialogue.ExecuteCommand(param))
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
            if (uiMode == UISelectMode.Out)
            {
                var choice = Instantiate(_outButtonPrefab, outButtonGroup.transform, false);
                foreach (var chosenText in choice.GetChoiceTexts())
                    chosenText.text = text;

                choice.ReSizeToFitChildren();
                SetBtnSizeAfterContentSizeFitter();

                return choice.GetDialogueButton();
            }
            else
            {
                var choice = Instantiate(inButtonPrefab, inButtonGroup.transform, false);
                TextMeshProUGUI[] choiceText = choice.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var chosenText in choiceText)
                    chosenText.text = text;
                return choice;
            }
            return null!;
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

        private void SetActor(string actorId)
        {
            var actor = ResourceSystem.instance.GetActor(actorId);
            var portrait = actor.GetPortrait("Default"); // TODO: 여러 표정 초상화 지원 필요

            nameText.text = actor.name;

            if (portrait?.sprite == null)
            {
                talkImage.gameObject.SetActive(false);
            }
            else
            {
                talkImage.sprite = portrait.sprite;
                talkImage.rectTransform.anchoredPosition = _talkImageOriginalPosition + new Vector2(portrait.offsetX, portrait.offsetY);
                talkImage.SetNativeSize();
                talkImage.gameObject.SetActive(true);
            }
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
                    Game.Instance.Dialogue.StartStory(quest.startPrologue);
                });
            }

            if (quests.Length < 1)
            {
                uiMode = UISelectMode.Out;

                CreateContentView("더이상 진행 가능한 퀘스트가 없습니다.");
                Button choice = CreateChoiceView("확인");
                choice.onClick.AddListener(delegate
                {
                    Game.Instance.Dialogue.StopStory();
                });
            }
        }
    }
}
