using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour
{
    GameObject Target;
    Rigidbody2D m_rigidbody;
    Animator animator;

    [SerializeField] float speed = 2f;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rigidbody.gravityScale != 4) return;

        if (m_rigidbody.velocity.y < -12) m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, -12);

        if (Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Platform")) != null)
        {
            m_rigidbody.gravityScale = 0;
            m_rigidbody.velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().enabled = true;
        }


    }
    IEnumerator Chase()
    {
        animator.SetBool("Move", true);
        animator.SetTrigger("Find");
        Vector2 direction = (Target.transform.position - transform.position).normalized;
        float see = direction.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(see * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(1f);

        if (!Target) { animator.SetBool("Move", false); yield break; }

        animator.SetTrigger("Chase");
        GetComponent<BoxCollider2D>().enabled = false;
        m_rigidbody.gravityScale = 0;
        while (Target)
        {
            direction = (Target.transform.position - transform.position).normalized;
            m_rigidbody.velocity = direction * speed;
            see = direction.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(see * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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
}
