using System;
using Ciart.Pagomoa.Logger.ProcessScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Book
{
    public class QuestListItemUI: MonoBehaviour
    {
        public Sprite emptyBackgroundSprite;
        
        public Sprite defaultBackgroundSprite;
        
        public Sprite selectedBackgroundSprite;
        
        public Image progressBar;
        
        public TextMeshProUGUI titleText;
        
        public UnityEvent onClick;
        
        private Image _image;
        
        private bool _isSelected;
        
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _image.sprite = _isSelected ? defaultBackgroundSprite : selectedBackgroundSprite;
            }
        }
        
        public string questId
        {
            get;
            private set;
        }

        public void UpdateUI(ProcessQuest quest)
        {
            questId = quest.id;
            
            titleText.text = quest.title;
        }
        
        private void Awake()
        {
            _image = GetComponent<Image>();
        }
    }
}