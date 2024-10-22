using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems
{
    public class InteractableObject : MonoBehaviour
    {
        public Vector3 uiOffset = new Vector3(0f, 2f, 0f);

        // 유니티 이벤트 호출
        public UnityEvent interactionEvent; 

        private SpriteRenderer _spriteRenderer;

        private GameObject _interactableUI;

        private const string Outline = "_OutlineColor";
        private static readonly int OutlineColor = Shader.PropertyToID(Outline);
    
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _interactableUI = Game.Get<UIManager>().CreateInteractableUI(transform);
            _interactableUI.SetActive(false);
            _interactableUI.transform.position += uiOffset;
        }

        public void ActiveObject()
        {
            if (interactionEvent.GetPersistentEventCount() == 0) return;
            if (interactionEvent.GetPersistentListenerState(0)== UnityEventCallState.Off) return;
            
            var color = new Color(0.38f,0.75f, 0.92f, 1f);
        
            _spriteRenderer.material.SetColor(OutlineColor, color);
            _interactableUI.SetActive(true);
        }

        public void DisableObject()
        {
            if (interactionEvent.GetPersistentEventCount() == 0) return;
            if (interactionEvent.GetPersistentListenerState(0)== UnityEventCallState.Off) return;
            
            _spriteRenderer.material.SetColor(OutlineColor, Color.white);
            _interactableUI.SetActive(false);
        }

        public void LockInteraction()
        {
            _interactableUI.SetActive(false);
            
            var eventIndex = interactionEvent.GetPersistentEventCount();

            for (int i = 0; i < eventIndex; i++)
            {
                interactionEvent.SetPersistentListenerState(i, UnityEventCallState.Off);
            }
        }

        public void UnlockInteraction()
        {
            var eventIndex = interactionEvent.GetPersistentEventCount();

            for (int i = 0; i < eventIndex; i++)
            {
                interactionEvent.SetPersistentListenerState(i, UnityEventCallState.RuntimeOnly);
            }
        }
    }
}
