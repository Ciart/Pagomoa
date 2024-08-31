using System;
using Ciart.Pagomoa.Logger.ProcessScripts;
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
                
                if (questId == "") return;
                
                _image.sprite = _isSelected ? selectedBackgroundSprite : defaultBackgroundSprite;
                progressBarFill.sprite = _isSelected ? selectedProgressFillSprite : defaultProgressFillSprite;
                titleText.color = _isSelected ? selectedTextColor : defaultTextColor;
                titleTextShadow.enabled = !_isSelected;
                progressText.color = _isSelected ? selectedTextColor : defaultTextColor;
            }
        }
        
        public string questId
        {
            get;
            private set;
        }
        
        public void UpdateUI([CanBeNull] ProcessQuest quest)
        {
            if (quest is null)
            {
                _image.sprite = emptyBackgroundSprite;
                _image.type = Image.Type.Tiled;

                questId = "";
                
                progressBar.gameObject.SetActive(false);
                titleText.gameObject.SetActive(false);
                progressText.gameObject.SetActive(false);

                return;
            }

            _image.sprite = defaultBackgroundSprite;
            _image.type = Image.Type.Sliced;
            
            questId = quest.id;
            titleText.text = quest.title;
            progressText.text = $"{(int) (quest.progress * 100)}%";

            progressBar.gameObject.SetActive(true);
            titleText.gameObject.SetActive(true);
            progressText.gameObject.SetActive(true);
            
            progressBar.value = quest.progress;
            progressBarFill.enabled = quest.progress != 0; // 진행도 0%일 경우 아예 비활성화
            
            // TODO: 완료된 퀘스트 표시
        }

        protected override void Awake()
        {
            base.Awake();
            
            _image = GetComponent<Image>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (questId == "") return;
            
            onClick.Invoke(questId);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (questId == "") return;
            
            onClick.Invoke(questId);
        }
    }
}