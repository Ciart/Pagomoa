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
            if (!collision.GetComponent<InteractableObject>()) return;

            if (!interactableObjectList.Contains(collision.GetComponent<InteractableObject>()))
                interactableObjectList.Add(collision.GetComponent<InteractableObject>());
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<InteractableObject>())
            {
                collision.GetComponent<InteractableObject>().DisableObject();
                interactableObjectList.Remove(collision.GetComponent<InteractableObject>());
            }
        }
        private void CheckInteractable(InteractableObject obj)
        {
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
