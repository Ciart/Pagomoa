using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour
{
    GameObject Target;
    Rigidbody2D m_rigidbody;
    Animator animator;

    private Monster _monster;

    private BoxCollider2D _boxCollider2D;

    private float _speed;

    private Vector2 _direction; 
    private float _see;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _monster = GetComponent<Monster>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _speed = _monster.moveSpeed;
    }
    
    void Update()
    {
        if (m_rigidbody.gravityScale != 4) return;

        if (m_rigidbody.velocity.y < -12) m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, -12);

        if (!Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Platform")))
        {
            m_rigidbody.gravityScale = 0;
            m_rigidbody.velocity = Vector2.zero;
            _boxCollider2D.enabled = true;
        }
    }
    IEnumerator Chase()
    {
        FindTarget();
        
        yield return new WaitForSeconds(1f);

        if (!Target) { animator.SetBool("Move", false); yield break; } // 타겟 1초동안 없으면 원래대로

        SetUpFlying();
        
        while (Target)
        {
            ChaseTarget();
            yield return null;
        }
        
        animator.SetBool("Move", false);
        m_rigidbody.velocity = Vector3.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            Target = collision.gameObject;
            StartCoroutine("Chase");
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Target)
        {
            Target = null;
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.gravityScale = 4;
        }
    }

    private void FindTarget()
    {
        animator.SetBool("Move", true);
        animator.SetTrigger("Find");
        Vector2 direction = (Target.transform.position - transform.position).normalized;
        _see = direction.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(_see * Mathf.Abs(transform.localScale.x), 1f, 1f);
    }

    private void SetUpFlying()
    {
        animator.SetTrigger("Chase");
        _boxCollider2D.enabled = false;
        m_rigidbody.gravityScale = 0;
    }

    private void ChaseTarget()
    {
        _direction = (Target.transform.position - transform.position).normalized;
        m_rigidbody.velocity = _direction * _speed;
        _see = _direction.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(_see * Mathf.Abs(transform.localScale.x), 1f, 1f);
    }
}
