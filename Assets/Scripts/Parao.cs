using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parao : MonoBehaviour
{
    GameObject Target;
    Rigidbody2D m_rigidbody;
    Animator animator;

    bool canAttack = true;
    [SerializeField] float attack_cooltime = 1f;
    [SerializeField] float speed = 2f;
    [SerializeField] float attack_distance = 1f;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Target == null) return;

        Vector2 direction = (Target.transform.position - transform.position).normalized;
        if (canAttack)
            m_rigidbody.velocity = direction * speed;
        else
            m_rigidbody.velocity = Vector3.zero;
        float see = direction.x > 0 ? 1 : -1;
        
        transform.localScale = new Vector3(see * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, attack_distance);

        foreach(Collider2D n in collider)
        {
            if (n.gameObject == Target && canAttack)
                Attack();
        }
    }
    void Attack()
    {
        animator.SetTrigger("Attack");
        StartCoroutine("Cooltimer", attack_cooltime);
    }
    IEnumerator Cooltimer(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.name == "Player")
            Target = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Target)
        {
            Target = null;
            Debug.Log("이젠 타겟아냐");
            m_rigidbody.velocity = Vector3.zero;
        }
    }
}
