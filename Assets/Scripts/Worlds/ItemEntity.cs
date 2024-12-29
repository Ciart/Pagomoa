using System;
using System.Collections;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds
{
    public class ItemEntity: MonoBehaviour
    {
        public float initDelay = 0.1f;

        /// <summary>
        /// 흔들리는 진폭을 결정합니다.
        /// </summary>
        public float amplitude = 0.1f;

        /// <summary>
        /// 흔들리는 속도를 결정합니다.
        /// </summary>
        public float frequency = 1f;
        
        public SpriteRenderer spriteRenderer;
    
        private Rigidbody2D _rigidbody;

        private float _time;
        
        private Item _item;
        
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                spriteRenderer.sprite = _item.sprite;
            }
        }

        public ItemEntity InstantiateItem(ItemEntity itemEntity, Vector3 position)
        {
            var item = Instantiate(itemEntity, position, Quaternion.identity);
            return item;
        } 

        private IEnumerator ChangeDynamicWithDelay()
        {
            yield return new WaitForSeconds(initDelay);

            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _time = 0f;
            
            StartCoroutine(ChangeDynamicWithDelay());
        }

        private void Update()
        {
            _time += Time.deltaTime;
            
            var y = Mathf.Cos(_time * Mathf.PI * frequency) * amplitude + amplitude;
            
            var image = spriteRenderer.gameObject.transform;
            image.localPosition = new Vector3(image.localPosition.x, y, image.localPosition.z);
        }
    }
}
