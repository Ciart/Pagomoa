using System.Collections;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemEntity: MonoBehaviour
    {
        public float initDelay = 0.1f;
        
        private Rigidbody2D _rigidbody;
    
        private SpriteRenderer _spriteRenderer;

        private Item _item;
        
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                _spriteRenderer.sprite = _item.itemImage;
            }
        }

        private IEnumerator ChangeDynamicWithDelay()
        {
            yield return new WaitForSeconds(initDelay);

            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(ChangeDynamicWithDelay());
        }
    }
}