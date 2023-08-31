using System;
using System.Collections;
using System.Collections.Generic;
using UFO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerInteractWithObject : MonoBehaviour
{
    [SerializeField] float interactableDistance = 1.5f;
    [SerializeField] private List<InteractableObject> _interactableObjectList;
    private InteractableObject _activatedObject;

    void Awake()
    {
        _interactableObjectList = new List<InteractableObject>();
        GetComponent<CircleCollider2D>().radius = interactableDistance;
        GetComponentInParent<Player.PlayerInput>().Actions.Interaction.started += context => 
        {
            if (_activatedObject) _activatedObject.InteractionEvent.Invoke();
        };
    }

    void FixedUpdate()
    {
        if(_interactableObjectList.Count > 0)
            CheckInteractable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableObject interobject = collision.GetComponent<InteractableObject>();
        
        if (!interobject) return;
        
        if (!_interactableObjectList.Contains(interobject))
            _interactableObjectList.Add(interobject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        InteractableObject interobject = collision.GetComponent<InteractableObject>();

        if (!interobject) return;
        
        interobject.DisableObject();

        if (_interactableObjectList.Contains(interobject))
        {
            if(_activatedObject == interobject)
                _activatedObject = null;

            _interactableObjectList.Remove(interobject);
        }
    }
        
    private void CheckInteractable()
    {
        float minDistance = 1000f;
        InteractableObject closestObject = null;
        foreach (InteractableObject obj in _interactableObjectList)
        {
            float objDistance = Vector2.Distance(transform.position, obj.transform.position);

            if (objDistance < minDistance)
            {
                minDistance = objDistance;
                closestObject = obj;
            }
        }
        if (_activatedObject)
        {
            _activatedObject.DisableObject();
            _activatedObject = null;
        }
        if (closestObject)
        {
            _activatedObject = closestObject;
            closestObject.ActiveObject();
        }
        //float distance = Vector2.Distance(transform.position, obj.transform.position);


        //if (_closestDistance == 0.0f || _interactableObjectList.Count == 1)
        //{
        //    _closestDistance = distance;
        //    _activatedObject = obj;
        //    obj.ActiveObject();
        //}
        //else if (distance < _closestDistance && _interactableObjectList.Count > 1)
        //{
        //    _activatedObject.DisableObject();
        //    _activatedObject = obj;
        //    _closestDistance = distance;
        //    obj.ActiveObject();
        //} 
        //if (_interactableObjectList.Count == 0) _activatedObject = null;
    }
        
}
