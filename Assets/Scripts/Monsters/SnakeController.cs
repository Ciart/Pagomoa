using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private GameObject _target;
    
    private Rigidbody2D _rigidbody;
    
    private Animator _animator;
    
    private float _speed;
    
    private Monster _monster;
    
    private SpriteRenderer _sleepingAnimation;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _monster = GetComponent<Monster>();
        _sleepingAnimation = transform.GetChild(0).GetComponent<SpriteRenderer>();

        _speed = _monster.moveSpeed;
        _sleepingAnimation.enabled = true;
    }
    void Update()
    {
        if (!_target) { return; }
        ChaseTarget();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name == "Player" && _monster.currentState != Monster.MonsterState.Sleep)
        {
            _sleepingAnimation.enabled = false;
            _target = collision.gameObject;
            _animator.SetTrigger("Chase");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _target && _monster.currentState != Monster.MonsterState.Sleep)
        {
            _target = null;
            _rigidbody.velocity = Vector3.zero;
            _animator.SetTrigger("Idle");
            if (_monster.currentState == Monster.MonsterState.WakeUpForaWhile)
            {
                _monster.currentState = Monster.MonsterState.Sleep;
                _sleepingAnimation.enabled = true;
            }
        }
    }

    private void ChaseTarget()
    {
        if (_monster.currentState == Monster.MonsterState.Active || _monster.currentState == Monster.MonsterState.WakeUpForaWhile)
        {
            Vector2 direction = (_target.transform.position - transform.position).normalized;
            _rigidbody.velocity = new Vector2(direction.x * _speed, 0);
        
            float forward = direction.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(forward * Mathf.Abs(transform.localScale.x), 1f, 1f);
            _sleepingAnimation.enabled = false;
        } else if (_monster.currentState == Monster.MonsterState.Sleep)
        {
            Sleeping();
        }
    }

    private void Sleeping()
    {
        _sleepingAnimation.enabled = true;
        _rigidbody.velocity = Vector3.zero;
        _animator.SetTrigger("Idle");
    }
}