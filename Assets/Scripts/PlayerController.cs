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

    public float oxygen_consume = 0.001f;

    public floatfloatEvent oxygen_alter;
    public floatfloatEvent hungry_alter;
    public floatEvent direction_alter;

    private Animator _animator;

    [System.Serializable]
    public class floatfloatEvent : UnityEvent<float, float> { }
    [System.Serializable]
    public class floatEvent : UnityEvent<float> { }
    // Update is called once per frame
    private void Awake()
    {
        if (oxygen_alter == null)
            oxygen_alter = new floatfloatEvent();
        if (hungry_alter == null)
            hungry_alter = new floatfloatEvent();
        if (direction_alter == null)
            direction_alter = new floatEvent();
        _animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    { 
        if (transform.position.y < GroundHeight && status.oxygen >= status.min_oxygen)
            status.oxygen -= Mathf.Abs(transform.position.y) * oxygen_consume;
        else
            if(status.oxygen < status.max_oxygen)
                status.oxygen += Mathf.Abs(transform.position.y) * oxygen_consume;
        oxygen_alter.Invoke(status.oxygen, status.max_oxygen);
    }
    public void SetDirection(float direction)
    {
        if (direction != 0)
        {
            WalkingAnimation(direction);
            Vector3 Direction = new Vector3(direction, 1, 1);
            transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z) ;
            direction_alter.Invoke(direction);
        }
        else
        {
            _animator.SetBool("goLeft", false);
            _animator.SetBool("goRight", false);
        }
    }
    public void WalkingAnimation(float direction)
    {
        if (direction == 1) { _animator.SetBool("goRight", true); }
        else { _animator.SetBool("goLeft", true); }
    }
    public void ActiveClimbingAnimation()
    {
        if (_animator.GetInteger("climb") == 0)
        {
            _animator.SetInteger("climb", 1);
        }
    }
    public void DisableClimbingAnimation()
    {
        if (_animator.GetInteger("climb") == 1 || _animator.GetInteger("climb") == 2)
        {
            _animator.SetInteger("climb", 0);
        }
    }
    public void ActiveLeftTileClimbingAnimation()
    {
        if (_animator.GetInteger("tileDirection") == 0)
        {
            _animator.SetInteger("climb", 2);
            _animator.SetInteger("tileDirection", -1);
        }
    }
    public void ActiveRightTileClimbingAnimation()
    {
        if (_animator.GetInteger("tileDirection") == 0)
        {
            _animator.SetInteger("climb", 2);
            _animator.SetInteger("tileDirection", 1);
        }
    }
    public void DisableTileClimbingAnimation()
    {
        if (_animator.GetInteger("tileDirection") == 1 || _animator.GetInteger("tileDirection") == -1)
        {
            _animator.SetInteger("tileDirection", 0);
        }
    }
    public void ActiveLeftTileClimbingEndMotion()
    {
        _animator.SetBool("endClimb", true);
    }
    public void DisableLeftTileClimbingEndMotion()
    {
        _animator.SetBool("endClimb", false);
    }
}
