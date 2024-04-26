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
        public GameObject talkPannel;
        public Image talkImage;
        public TextMeshProUGUI talkText;
        public TextMeshProUGUI nameText;

        [SerializeField]
        private GameObject outButtonGroup;
        [SerializeField]
        private GameObject inButtonGroup;

        public List<Sprite> spriteGroup;

        private static DialogueManager _instance = null;

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

        private TextAsset inkJSONAsset = null;
        private DialogueTrigger nowTrigger;

        public Story story;

        [SerializeField]
        private Button outButtonPrefab = null;
        [SerializeField]
        private Button inButtonPrefab = null;

        enum UISelectMode {
            In,
            Out
        }
        
        UISelectMode uiMode = UISelectMode.Out;
        bool changeDialogue = false;

        private void Update()
        {
            SetBtnSizeAfterContentSizeFitter();
        }

        private void SetBtnSizeAfterContentSizeFitter()
        {
            if (uiMode == UISelectMode.Out)
            {
                var rect = outButtonGroup.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector3(162f - rect.sizeDelta.x * 0.5f, 47f + rect.sizeDelta.y * 0.5f);
            }
        }

        public void SetJsonAsset(TextAsset asset)
        {
            inkJSONAsset = asset;
        }

        public void SetDialogueTrigger(DialogueTrigger trigger)
        {
            nowTrigger = trigger;
        }

        public void StartStory()
        {
            story = new Story(inkJSONAsset.text);
            if (onCreateStory != null) onCreateStory(story);
            RefreshView();
            talkPannel.SetActive(true);
        }

        public void StopStory()
        {
            talkPannel.SetActive(false);
        }

        private void RefreshView()
        {
            RemoveChildren(outButtonGroup);
            RemoveChildren(inButtonGroup);

            // Read all the content until we can't continue any more
            while (story.canContinue)
            {
                // Continue gets the next line of the story
                string text = story.Continue();
                // This removes any white space from the text.
                text = text.Trim();

                // Display the text on screen!
                CreateContentView(text);
            }
            ParseTag();
            
            if (changeDialogue == true)
            {
                changeDialogue = false;
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
            var choice = Instantiate(uiMode == UISelectMode.Out ? outButtonPrefab : inButtonPrefab);

            TextMeshProUGUI[] choiceText = choice.GetComponentsInChildren<TextMeshProUGUI>();
            foreach(var chosedtext in choiceText)
                chosedtext.text = text;


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

        private void ParseTag()
        {
            List<string> tags = new List<string>();
            tags = story.currentTags;

            foreach(var tag in tags)
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
                        if (param == "In") uiMode = UISelectMode.In;
                        if (param == "Out") uiMode = UISelectMode.Out;
                        break;
                    case "start":
                        if (param == "dialogue")
                        {
                            nowTrigger.StartDialogue();
                            changeDialogue = true;
                        }
                        if (param == "quest")
                        {
                            SelectQuest(nowTrigger.GetQuestDialogue());
                            changeDialogue = true;
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

        private void SelectQuest(QuestDialogue[] quests)
        {
            uiMode = UISelectMode.In;
            foreach (var quest in quests)
            {
                Button button = CreateChoiceView(quest.questName);
                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate
                {
                    SetJsonAsset(quest.questStartPrologos);
                    StartStory();
                });
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

            foreach(var sprite in spriteGroup)
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

        private void QuestAccept(string id)
        {
            var questId = int.Parse(id);
            var interact = nowTrigger.GetComponent<InteractableObject>();

            var questDialogues = nowTrigger.GetQuestDialogue();
            
            
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
            var interact = nowTrigger.GetComponent<InteractableObject>();
            
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
