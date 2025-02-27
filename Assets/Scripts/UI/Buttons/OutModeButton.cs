using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class OutModeButton : MonoBehaviour
    {
        public RectTransform rectTransform;
        [SerializeField] private DialogueButton _dialogueButton;
        public DialogueButton GetDialogueButton() { return _dialogueButton; }
        [SerializeField] private TextMeshProUGUI[] _choiceTexts = new TextMeshProUGUI[2];
        public TextMeshProUGUI[] GetChoiceTexts() { return _choiceTexts; }
        
        private RectTransform _dialogueButtonRectTransform;
        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            
            if (_choiceTexts[0] == null || _choiceTexts[1] == null)
                _choiceTexts = GetComponentsInChildren<TextMeshProUGUI>();

            _dialogueButtonRectTransform = _dialogueButton.GetComponent<RectTransform>();
        }
        
        public void ReSizeToFitChildren()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_dialogueButtonRectTransform);
            rectTransform.sizeDelta = new Vector2(_dialogueButtonRectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        }
    }
}
