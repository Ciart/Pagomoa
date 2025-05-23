using System;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Book
{
    public class QuestListItemUI: UIBehaviour, IPointerClickHandler, ISubmitHandler
    {
        public Sprite emptyBackgroundSprite;
        
        public Sprite defaultBackgroundSprite;
        
        public Sprite selectedBackgroundSprite;
        
        public Sprite defaultProgressFillSprite;
        
        public Sprite selectedProgressFillSprite;

        public Sprite completedSprite;
        
        public Color defaultTextColor;
        
        public Color selectedTextColor;
        
        public Slider progressBar;
        
        public Image progressBarFill;
        
        public TextMeshProUGUI titleText;
        
        public TextMeshProUGUI titleTextShadow;
        
        public TextMeshProUGUI progressText;
        
        public Action<string> onClick;
        
        private Image _image;
        
        private bool _isSelected;
        
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;

                var quest = Game.Instance.Quest.FindQuestById(questID);
                if (questID == "") return;
                if (quest.state == QuestState.Finish)
                {
                    progressText.text = "완료";
                    progressText.color = _isSelected ? 
                        new Color32(0x8f, 0xf8, 0xe2, 255) : Color.white; 
                    titleText.color = _isSelected ? 
                        new Color32(0x8f, 0xf8, 0xe2, 255) : Color.white;
                    _image.sprite = completedSprite;
                    _image.type = Image.Type.Tiled;
                    return;
                }
                
                _image.sprite = _isSelected ? selectedBackgroundSprite : defaultBackgroundSprite;
                progressBarFill.sprite = _isSelected ? selectedProgressFillSprite : defaultProgressFillSprite;
                titleText.color = _isSelected ? selectedTextColor : defaultTextColor;
                titleTextShadow.enabled = !_isSelected;
                progressText.color = _isSelected ? selectedTextColor : defaultTextColor;
            }
        }
        
        public string questID
        {
            get;
            private set;
        }
        
        public void UpdateUI(Quest? quest)
        {
            if (quest is null)
            {
                _image.sprite = emptyBackgroundSprite;
                _image.type = Image.Type.Tiled;

                questID = "";
                
                progressBar.gameObject.SetActive(false);
                titleText.gameObject.SetActive(false);
                progressText.gameObject.SetActive(false);

                return;
            }
            
            questID = quest.id;
            titleText.text = quest.title;
            progressText.text = $"{(int) (quest.progress * 100)}%";
            _image.sprite = defaultBackgroundSprite;
            _image.type = Image.Type.Sliced;
            
            progressBar.gameObject.SetActive(true);
            titleText.gameObject.SetActive(true);
            progressText.gameObject.SetActive(true);
            
            progressBar.value = quest.progress;
            progressBarFill.enabled = quest.progress != 0 && quest.state != QuestState.Finish;
        }

        protected override void Awake()
        {
            base.Awake();
            
            _image = GetComponent<Image>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (questID == "") return;
            
            onClick.Invoke(questID);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (questID == "") return;
            
            onClick.Invoke(questID);
        }
    }
}