using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    GameObject Target;
    Rigidbody2D m_rigidbody;
    Animator animator;
    private float _speed;
    private Monster _monster;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _monster = GetComponent<Monster>();
        
        _speed = _monster.moveSpeed;
    }
    void Update()
    {
        if (!Target) { return; }
        
        Vector2 direction = (Target.transform.position - transform.position).normalized;
        m_rigidbody.velocity = new Vector2(direction.x * _speed, 0);

        float forward = direction.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(forward * Mathf.Abs(transform.localScale.x), 1f, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            Target = collision.gameObject;
            animator.SetTrigger("Chase");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Target)
        {
            Target = null;
            m_rigidbody.velocity = Vector3.zero;
            animator.SetTrigger("Idle");
        }
    }
}