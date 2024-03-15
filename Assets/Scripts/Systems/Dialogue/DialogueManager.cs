using System;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEditor.U2D.Aseprite;
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
        private GameObject buttonGroup;

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
        public Story story;

        [SerializeField]
        private Button buttonPrefab = null;

        private List<Button> pool;


        private void Awake()
        {
            Debug.Log(spriteGroup.Count);
            Debug.Log(spriteGroup[0].name);
        }

        public void SetJsonAsset(TextAsset asset)
        {
            inkJSONAsset = asset;
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
            Debug.Log("새 화면~!");
            RemoveChildren(buttonGroup);

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
            ParseTag();
        }

        // Creates a button showing the choice text
        private Button CreateChoiceView(string text)
        {
            Button choice = Instantiate(buttonPrefab) as Button;
            choice.transform.SetParent(buttonGroup.transform, false);

            Text choiceText = choice.GetComponentInChildren<Text>();
            choiceText.text = text;

            HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
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

                }
            }
        }

        
        private void SetTalkerName(string name)
        {
            Debug.Log("말하는사람: " + name);
            nameText.text = name;
        }

        private void SetSpriteImage(string param)
        {
            Debug.Log("이미지: " + param);
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

    }
}
