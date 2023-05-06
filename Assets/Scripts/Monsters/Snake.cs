using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Snake : MonoBehaviour
{
    GameObject Target;
    Rigidbody2D m_rigidbody;
    Animator animator;
    [SerializeField] float speed = 2f;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
            return;
        Vector2 direction = (Target.transform.position - transform.position).normalized;
        m_rigidbody.velocity = direction * speed;

        float forward = direction.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(forward * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            Target = collision.gameObject;
            animator.SetTrigger("Chase");
            Debug.Log("트리거엔터");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Target)
        {
            Target = null;
            m_rigidbody.velocity = Vector3.zero;
            animator.SetTrigger("Idle");
            Debug.Log("트리거익시트");
        }
    }
}
