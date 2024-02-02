using System;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public List<Dialogue> dialogues;

        [Space]
        public GameObject talkPannel;
        public Image talkImage;
        public TextMeshProUGUI talkText;
        public TextMeshProUGUI nameText;

        [SerializeField]
        private GameObject buttonGroup;

        // <Dialogue Version>
        [Space]
        public Dialogue NowScenario;
        int talkIndex;
        [Space]
        private static DialogueManager _instance = null;
        public static DialogueManager Instance
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

        private Dialogue GetDialogueByID(int id)
        {
            foreach(Dialogue dialogue in dialogues)
            {
                if (dialogue.id == id)
                    return dialogue;
            }
            return null;
        }

        public bool ConversationProgress(int id)
        {
            NowScenario = GetDialogueByID(id);

            if(NowScenario.talk.Count <= talkIndex)
            {
                talkIndex = 0;
                NowScenario = null;
                talkPannel.SetActive(false);
                return false;
            }
            talkText.text = talkIndex < NowScenario.talk.Count ? NowScenario.talk[talkIndex] : "";
            talkImage.sprite = talkIndex < NowScenario.sprite.Count ? NowScenario.sprite[talkIndex] : null ;
            nameText.text = talkIndex < NowScenario.talkerName.Count ? NowScenario.talkerName[talkIndex] : "";
            talkImage.enabled = talkImage.sprite == null ? false : true;
            bool visible = nameText.text == "" ? false : true;
            nameText.transform.parent.gameObject.SetActive(visible);

            talkPannel.SetActive(true);
            talkIndex++;
            return true;
        }
        // </>


        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////
        /// </summary>

        public static event Action<Story> OnCreateStory;

        // Creates a new Story object with the compiled story which we can then play!

        public void SetJsonAsset(TextAsset asset)
        {
            inkJSONAsset = asset;
        }
        public void StartStory()
        {
            story = new Story(inkJSONAsset.text);
            if (OnCreateStory != null) OnCreateStory(story);
            RefreshView();
            talkPannel.SetActive(true);
        }
        public void StopStory()
        {
            talkPannel.SetActive(false);
        }
        private void RefreshView()
        {
            buttonGroup.SetActive(false);
            // Remove all the UI on screen
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
                buttonGroup.SetActive(true);
            }
            // If we've read all the content and there's no choices, the story is finished!
            else
            {
                Button choice = CreateChoiceView("확인");
                choice.onClick.AddListener(delegate
                {
                    StopStory();
                });
                buttonGroup.SetActive(true);
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
            //Text storyText = Instantiate(textPrefab) as Text;
            talkText.text = text;
            //storyText.transform.SetParent(canvas.transform, false);
        }

        // Creates a button showing the choice text
        private Button CreateChoiceView(string text)
        {
            // Creates the button from a prefab
            Button choice = GetButton();
            choice.transform.SetParent(buttonGroup.transform, false);

            // Gets the text from the button prefab
            Text choiceText = choice.GetComponentInChildren<Text>();
            choiceText.text = text;
            // Make the button expand to fit the text
            HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;

            return choice;
        }

        // Object Pooling
        private Button GetButton()
        {
            if (pool == null)
                pool = new List<Button>();

            Button select = null;

            foreach (var item in pool)
            {
                if (!item.gameObject.activeSelf)
                {
                    select = item;
                    select.onClick.RemoveAllListeners();
                    select.gameObject.SetActive(true);
                    break;
                }
            }
            if (!select)
            {
                select = Instantiate(buttonPrefab, transform);
                pool.Add(select);
            }

            return select;
        }

        // Destroys all the children of this gameobject (all the UI)
        private void RemoveChildren(GameObject vas)
        {
            int childCount = vas.transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                vas.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private TextAsset inkJSONAsset = null;
        public Story story;


        [SerializeField]
        private Button buttonPrefab = null;

        private List<Button> pool;
    }
}
