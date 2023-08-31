using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerInteractWithObject : MonoBehaviour
{
<<<<<<< HEAD
<<<<<<< HEAD
    [SerializeField] private List<InteractableObject> interactableObjectList;
    private float _closestDistance;
    private InteractableObject _activatedObject;

    public bool getKey;

    private Player.PlayerInput _playerInput;
        //Key.F;
    
    void Start()
=======
    [SerializeField] float interactableDistance = 1.5f;
    [SerializeField] private List<InteractableObject> _interactableObjectList;
    private InteractableObject _activatedObject;

    void Awake()
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
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
<<<<<<< HEAD
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    }

    void FixedUpdate()
    {
<<<<<<< HEAD
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
=======
        if(_interactableObjectList.Count > 0)
            CheckInteractable();
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableObject interobject = collision.GetComponent<InteractableObject>();
        
<<<<<<< HEAD
<<<<<<< HEAD
        if (!interactableObjectList.Contains(collision.GetComponent<InteractableObject>()))
            interactableObjectList.Add(collision.GetComponent<InteractableObject>());
=======
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
        if (!interobject) return;
        
        if (!_interactableObjectList.Contains(interobject))
            _interactableObjectList.Add(interobject);
<<<<<<< HEAD
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
=======
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
<<<<<<< HEAD
            collision.GetComponent<InteractableObject>().DisableObject();
            interactableObjectList.Remove(collision.GetComponent<InteractableObject>());
=======
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
            if(_activatedObject == interobject)
                _activatedObject = null;

            _interactableObjectList.Remove(interobject);
<<<<<<< HEAD
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
        }
    }
        
    private void CheckInteractable()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        float distance = Vector2.Distance(transform.position, obj.transform.position);
        if (_closestDistance == 0.0f || interactableObjectList.Count == 1)
=======
        float minDistance = 1000f;
        InteractableObject closestObject = null;
        foreach (InteractableObject obj in _interactableObjectList)
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
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
=======
        if (_activatedObject)
        {
            _activatedObject.DisableObject();
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
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
<<<<<<< HEAD
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    }
        
}
