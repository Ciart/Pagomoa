using System;
using System.Collections;
using System.Collections.Generic;
using UFO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteractWithObject : MonoBehaviour
{
    [SerializeField] private List<InteractableObject> _interactableObjectList;
    private float _closestDistance;
    private InteractableObject _activatedObject;

    public bool getKey;
    public Key eventKey = Key.F;

    private Player.PlayerInput playerInput;
        //Key.E;
    
    void Start()
    {
        _interactableObjectList = new List<InteractableObject>();
        playerInput = GetComponentInParent<Player.PlayerInput>();
    }

    void FixedUpdate()
    {
        InputEventKey();

        if ( getKey && _activatedObject )
        {
            Debug.Log("??" + _activatedObject);
            _activatedObject.InteractionEvent.Invoke();
            Debug.Log("???" + _activatedObject);
        }

        foreach ( InteractableObject obj in _interactableObjectList )
        {
            CheckInteractable(obj);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<InteractableObject>()) return;
        
        if (!_interactableObjectList.Contains(collision.GetComponent<InteractableObject>()))
            _interactableObjectList.Add(collision.GetComponent<InteractableObject>());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractableObject>())
        {
            collision.GetComponent<InteractableObject>().DisableObject();
            _interactableObjectList.Remove(collision.GetComponent<InteractableObject>());
        }
    }
    private void CheckInteractable(InteractableObject obj)
    {
        float distance = Vector2.Distance(transform.position, obj.transform.position);
        if (_closestDistance == 0.0f || _interactableObjectList.Count == 1)
        {
            _closestDistance = distance;
            _activatedObject = obj;
            obj.ActiveObject();
        }
        else if (distance < _closestDistance && _interactableObjectList.Count > 1)
        {
            _activatedObject.DisableObject();
            _activatedObject = obj;
            _closestDistance = distance;
            obj.ActiveObject();
        } 
        if (_interactableObjectList.Count == 0) _activatedObject = null;
    }
    
    private void InputEventKey()
    {
        if (playerInput.IsInteraction)
            getKey = true;
        else
            getKey = false;
    }
}
