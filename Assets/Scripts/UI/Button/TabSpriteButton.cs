using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI
{
    public class TabSpriteButton: UIBehaviour, IPointerClickHandler, ISubmitHandler
    {
        public Sprite defaultSprite;
        
        public Sprite selectedSprite;

        public UnityEvent onClick;
     
        private Image _image;
        
        private bool _isSelected;
        
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _image.sprite = _isSelected ? selectedSprite : defaultSprite;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            _image = GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            onClick.Invoke();
        }
    }
}