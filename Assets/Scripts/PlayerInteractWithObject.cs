using System;
using System.Collections;
using System.Collections.Generic;
using UFO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractWithObject : MonoBehaviour
{
    private List<InteractableObject> _InteractableObjectList;
    private float _closestDistance;
    private InteractableObject _ActivatedObject;

    public bool getKey;
    public KeyCode eventKey = KeyCode.E;
    
    void Start()
    {
        _InteractableObjectList = new List<InteractableObject>();
    }
    void FixedUpdate()
    {
        InputEventKey();

        if ( getKey && _ActivatedObject )
        {
            _ActivatedObject.InteractionEvent.Invoke();
        }

        foreach ( InteractableObject obj in _InteractableObjectList )
        {
            CheckInteractable(obj);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractableObject>())
        {
            if (!_InteractableObjectList.Contains(collision.GetComponent<InteractableObject>()))
            {
                _InteractableObjectList.Add(collision.GetComponent<InteractableObject>());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractableObject>())
        {
            collision.GetComponent<InteractableObject>().DisableObject();
            _InteractableObjectList.Remove(collision.GetComponent<InteractableObject>());
        }
    }
    private void CheckInteractable(InteractableObject obj)
    {
        float distance = Vector2.Distance(transform.position, obj.transform.position);
        if (_closestDistance == 0.0f || _InteractableObjectList.Count == 1)
        {
            _closestDistance = distance;
            obj.ActiveObject();
            _ActivatedObject = obj;
        }
        else if (distance < _closestDistance && _InteractableObjectList.Count > 1)
        {
            _ActivatedObject.DisableObject();
            _ActivatedObject = obj;
            _closestDistance = distance;
            obj.ActiveObject();
        } 
        if (_InteractableObjectList.Count == 0) _ActivatedObject = null;
    }
    
    private void InputEventKey()
    {
        if (Input.GetKey(eventKey))
        {
            getKey = true;
        }
        else
        {
            getKey = false;
        }
    }
    
}
