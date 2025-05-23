using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class CutSceneDialogue : MonoBehaviour
    {
        public Story story;

        public GameObject dialoguePanel;
        
        public GameObject outButtonGroup;
        public GameObject inButtonGroup;
        public OutModeButton outButtonPrefab;
        public Button inButtonPrefab;
        
        public Image talkImage;
        public TextMeshProUGUI talkText;
        public TextMeshProUGUI nameText;
        public List<Sprite> spriteGroup;
        
        public UISelectMode uiMode = UISelectMode.Out;
        private bool _changeDialogue;
        
        [SerializeField] private PlayableDirector director;
        
        public void StartCutSceneStory(TextAsset storyAsset)
        {
            story = new Story(storyAsset.text);

            dialoguePanel.gameObject.SetActive(true);
            
            RefreshView();
        }
        
        public void StopStory()
        {
            story = null;
            dialoguePanel.gameObject.SetActive(false);
            
            director.Play();
        }
        
        private void RefreshView()
        {
            RemoveChildren(outButtonGroup);
            RemoveChildren(inButtonGroup);
            talkText.text = "";
            // Read all the content until we can't continue anymore
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
                    StopStory();
                });
            }
        }
        
        public void ParseTag()
        {
            List<string> currentTags = story.currentTags;

            foreach (var currentTag in currentTags)
            {
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
                }
            }
        }
        
        private void OnClickChoiceButton(Choice choice)
        {
            story.ChooseChoiceIndex(choice.index);

            RefreshView();
        }
        
        private void SetBtnSizeAfterContentSizeFitter()
        { 
            if (!story) return; // 상점과 같은 버튼이 없는 대화 출력시 버튼 에러 방지
            var buttonCount = story.currentChoices.Count;
            
            if (buttonCount == 0) buttonCount = 1; // 마지막 선택 시 때문에 설정
            outButtonGroup.TryGetComponent<RectTransform>(out var rect);
            var buttonSize = outButtonPrefab.rectTransform.sizeDelta;
            rect.anchoredPosition = new Vector2(0f, (buttonSize.y + 4)  * buttonCount);
        }
        
        private void CreateContentView(string text) { talkText.text = text; }
        
        private Button CreateChoiceView(string text)
        {
            var choice = Instantiate(outButtonPrefab , outButtonGroup.transform, false);
            foreach (var chosenText in choice.GetChoiceTexts())
                chosenText.text = text;
                
            choice.ReSizeToFitChildren();
            SetBtnSizeAfterContentSizeFitter();
                
            return choice.GetDialogueButton();
        }
        
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
    }
}
