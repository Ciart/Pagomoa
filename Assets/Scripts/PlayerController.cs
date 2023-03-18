using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Status))]
public class PlayerController : MonoBehaviour
{
    public Status status;

    public float GroundHeight = 0;

    public LayerMask WhatisGround;

    public UnityEvent oxygen_alter;
    public UnityEvent hungry_alter;
    public UnityEvent direction_alter;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    // Update is called once per frame
    private void Awake()
    {
        if (oxygen_alter == null)
            oxygen_alter = new UnityEvent();
        if (hungry_alter == null)
            hungry_alter = new UnityEvent();
    }
    void Update()
    {
        float declineSpeed = 0.05f;
        
        if (transform.position.y < GroundHeight && status.oxygen >= status.min_oxygen)
            status.oxygen -= Mathf.Abs(transform.position.y) * declineSpeed;
        else
            if(status.oxygen < status.max_oxygen)
                status.oxygen += Mathf.Abs(transform.position.y) * declineSpeed;
        oxygen_alter.Invoke();
    }
    public void SetDirection(float direction)
    {
        if (direction != 0)
        {
            Vector3 Direction = new Vector3(direction, 1, 1);
            transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z) ;
            direction_alter.Invoke();
        }
    }

}
