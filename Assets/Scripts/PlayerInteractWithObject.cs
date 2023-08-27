using System;
using System.Collections;
using System.Collections.Generic;
using UFO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractWithObject : MonoBehaviour
{
    private List<InteractableObject> _interactableObjectList;
    private float _closestDistance;
    private InteractableObject _activatedObject;

    public bool getKey;
    public KeyCode eventKey = KeyCode.E;
    
    void Start()
    {
        _interactableObjectList = new List<InteractableObject>();
    }

    void FixedUpdate()
    {
        InputEventKey();

        if ( getKey && _activatedObject )
        {
            _activatedObject.InteractionEvent.Invoke();
        }

        foreach ( InteractableObject obj in _interactableObjectList )
        {
            CheckInteractable(obj);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractableObject>())
        {
            if (!_interactableObjectList.Contains(collision.GetComponent<InteractableObject>()))
            {
                _interactableObjectList.Add(collision.GetComponent<InteractableObject>());
            }
        }
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
            obj.ActiveObject();
            _activatedObject = obj;
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
        if (Input.GetKeyDown(eventKey))
        {
            getKey = true;
        }
        else
        {
            getKey = false;
        }
    }
    
}
