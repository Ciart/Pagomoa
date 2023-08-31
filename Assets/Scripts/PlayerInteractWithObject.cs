using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteractWithObject : MonoBehaviour
{
    [SerializeField] private List<InteractableObject> interactableObjectList;
    private float _closestDistance;
    private InteractableObject _activatedObject;

    public bool getKey;

    private Player.PlayerInput _playerInput;
        //Key.F;
    
    void Start()
    {
        interactableObjectList = new List<InteractableObject>();
        _playerInput = GetComponentInParent<Player.PlayerInput>();
    }

    void FixedUpdate()
    {
        InputEventKey();

        if ( getKey && _activatedObject )
        {
            _activatedObject.InteractionEvent.Invoke();
        }

        foreach ( InteractableObject obj in interactableObjectList )
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
            _activatedObject.DisableObject();
            _activatedObject = obj;
            _closestDistance = distance;
            obj.ActiveObject();
        } 
        if (interactableObjectList.Count == 0) _activatedObject = null;
    }
    
    private void InputEventKey()
    {
        if (_playerInput.IsInteraction)
            getKey = true;
        else
            getKey = false;
    }
}
