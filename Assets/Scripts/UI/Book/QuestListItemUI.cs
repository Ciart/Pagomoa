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
        
        public Image progressBar;
        
        public TextMeshProUGUI titleText;
        
        public Action<string> onClick;
        
        private Image _image;
        
        private bool _isSelected;
        
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _image.sprite = _isSelected ? selectedBackgroundSprite : defaultBackgroundSprite;
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

                return;
            }

            _image.sprite = defaultBackgroundSprite;
            _image.type = Image.Type.Sliced;
            
            questId = quest.id;
            titleText.text = quest.title;

            progressBar.gameObject.SetActive(true);
            titleText.gameObject.SetActive(true);
        }

        protected override void Awake()
        {
            base.Awake();
            
            _image = GetComponent<Image>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke(questId);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            onClick.Invoke(questId);
        }
    }
}