using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems
{
    public class InteractableObject : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        private SpriteRenderer _clickRenderer;

        private readonly string _outline = "_OutlineColor";
        
        public UnityEvent interactionEvent;
        
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        
            // _clickRenderer는 항상 해당 객채의 0번째에 위치해야 한다.
            _clickRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _clickRenderer.enabled = false;
        }
        public void ActiveObject()
        {
            Color a = new Color(0.38f,0.75f, 0.92f, 1f);
        
            _spriteRenderer.material.SetColor(_outline, a);
            
            _clickRenderer.enabled = true;
        }

        public void DisableObject()
        {
            _spriteRenderer.material.SetColor(_outline, Color.white);
            
            _clickRenderer.enabled = false;
        }
    }
}
