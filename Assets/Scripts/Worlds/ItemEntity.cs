using UnityEngine;

namespace Worlds
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemEntity: MonoBehaviour
    {
        private Item _item;
    
        private SpriteRenderer _spriteRenderer;

        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                _spriteRenderer.sprite = _item.itemImage;
            }
        }
    
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}