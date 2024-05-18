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
    
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _interactableUI = UIManager.CreateInteractableUI(transform);
            _interactableUI.SetActive(false);
            _interactableUI.transform.position += uiOffset;
        }

        public void ActiveObject()
        {
            var color = new Color(0.38f,0.75f, 0.92f, 1f);
        
            _spriteRenderer.material.SetColor(Outline, color);
            _interactableUI.SetActive(true);
        }

        public void DisableObject()
        {
            _spriteRenderer.material.SetColor(Outline, Color.white);
            _interactableUI.SetActive(false);
        }
    }
}
