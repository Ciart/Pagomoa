using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerInteractWithObject : MonoBehaviour
{
<<<<<<< HEAD
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
=======
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
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    }

    void FixedUpdate()
    {
<<<<<<< HEAD
        InputEventKey();

        if ( getKey && _activatedObject )
        {
            _activatedObject.InteractionEvent.Invoke();
        }

        foreach ( InteractableObject obj in interactableObjectList )
        {
            CheckInteractable(obj);
        }
=======
        if(_interactableObjectList.Count > 0)
            CheckInteractable();
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableObject interobject = collision.GetComponent<InteractableObject>();
        
<<<<<<< HEAD
        if (!interactableObjectList.Contains(collision.GetComponent<InteractableObject>()))
            interactableObjectList.Add(collision.GetComponent<InteractableObject>());
=======
        if (!interobject) return;
        
        if (!_interactableObjectList.Contains(interobject))
            _interactableObjectList.Add(interobject);
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        InteractableObject interobject = collision.GetComponent<InteractableObject>();

        if (!interobject) return;
        
        interobject.DisableObject();

        if (_interactableObjectList.Contains(interobject))
        {
<<<<<<< HEAD
            collision.GetComponent<InteractableObject>().DisableObject();
            interactableObjectList.Remove(collision.GetComponent<InteractableObject>());
=======
            if(_activatedObject == interobject)
                _activatedObject = null;

            _interactableObjectList.Remove(interobject);
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
        }
    }
        
    private void CheckInteractable()
    {
<<<<<<< HEAD
        float distance = Vector2.Distance(transform.position, obj.transform.position);
        if (_closestDistance == 0.0f || interactableObjectList.Count == 1)
=======
        float minDistance = 1000f;
        InteractableObject closestObject = null;
        foreach (InteractableObject obj in _interactableObjectList)
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
        {
            float objDistance = Vector2.Distance(transform.position, obj.transform.position);

            if (objDistance < minDistance)
            {
                minDistance = objDistance;
                closestObject = obj;
            }
        }
<<<<<<< HEAD
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
=======
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
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    }
        
}
