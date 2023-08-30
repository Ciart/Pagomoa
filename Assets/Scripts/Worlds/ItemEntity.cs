using UnityEngine;

namespace Worlds
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemEntity: MonoBehaviour
    {
        public Item item;
    
        private SpriteRenderer _spriteRenderer;

        public Item Item
        {
            get => item;
            set
            {
                item = value;
                _spriteRenderer.sprite = item.itemImage;
            }
        }
    
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _spriteRenderer.sprite = item.itemImage;
        }
    }
}