using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractWithObject : MonoBehaviour
{
    List<GameObject> InteractableObjectList;
    float closestDistance;
    GameObject ActivatedObject;
    void Start()
    {
        InteractableObjectList = new List<GameObject>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(InteractableObjectList.Count);
        }

        foreach (GameObject obj in InteractableObjectList)
        {
            CheckInteractable(obj);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractableObject>())
        {
            if (!InteractableObjectList.Contains(collision.gameObject))
            {
                InteractableObjectList.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractableObject>())
        {
            collision.GetComponent<InteractableObject>().DisableObject();
            InteractableObjectList.Remove(collision.gameObject);
        }
    }
    private void CheckInteractable(GameObject obj)
    {
        float distance = Vector2.Distance(transform.position, obj.transform.position);
        if (closestDistance == 0.0f || InteractableObjectList.Count == 1)
        {
            closestDistance = distance;
            obj.GetComponent<InteractableObject>().ActiveObject();
            ActivatedObject = obj;
        }
        else if (distance < closestDistance && InteractableObjectList.Count > 1)
        {
            ActivatedObject.GetComponent<InteractableObject>().DisableObject();
            ActivatedObject = obj;
            closestDistance = distance;
            obj.GetComponent<InteractableObject>().ActiveObject();
        }
    }
}
