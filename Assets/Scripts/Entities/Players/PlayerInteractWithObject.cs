using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Players
{
    public class PlayerInteractWithObject : MonoBehaviour
    {
        [SerializeField] private List<InteractableObject> interactableObjectList;
        private float _closestDistance;
        private InteractableObject _activatedObject;
    
        void Start()
        {
            interactableObjectList = new List<InteractableObject>();
        }
        private void Awake()
        {
            GetComponentInParent<PlayerInput>().Actions.Interaction.started += context =>
            {
                if (!_activatedObject) return;
                _activatedObject.interactionEvent.Invoke();
            };
        }
        void FixedUpdate()
        {
            if (interactableObjectList.Count == 0)
            {
                _activatedObject = null;
                return;
            }

            foreach (InteractableObject obj in interactableObjectList)
            {
                CheckInteractable(obj);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var interactableObject = collision.GetComponent<InteractableObject>();
            
            if (!interactableObject) return;

            if (!interactableObjectList.Contains(interactableObject))
                interactableObjectList.Add(interactableObject);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            var interactableObject = collision.GetComponent<InteractableObject>();
            
            if (interactableObject)
            {
                interactableObject.DisableObject();
                interactableObjectList.Remove(interactableObject);
            }
        }
        private void CheckInteractable(InteractableObject obj)
        {
            if (obj.enabled == false) return;
            
            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (_closestDistance == 0.0f || interactableObjectList.Count == 1)
            {
                _closestDistance = distance;
                _activatedObject = obj;
                obj.ActiveObject();
            }
            else if (distance < _closestDistance && interactableObjectList.Count > 1)
            {
                _activatedObject?.DisableObject();
                _activatedObject = obj;
                _closestDistance = distance;
                obj.ActiveObject();
            }
        }
    }
}
