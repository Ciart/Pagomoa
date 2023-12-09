using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlController : MonsterController
{
    //private GameObject touchingTarget;

    Coroutine Proceeding;
    private void FixedUpdate()
    {
        if (touchingTarget)
        {
            switch (touchingTarget.tag)
            {
                case "Player":
                    _monster._attack._Attack(gameObject, touchingTarget, _monster.status.attackPower);
                    break;
            }
        }
    }
    private void Start()
    {
        StateChanged(Monster.MonsterState.Active);
    }
    public override void StateChanged(Monster.MonsterState state)
    {
        if (Proceeding != null)
            StopCoroutine(Proceeding);

        _monster.state = state;

        switch (state)
        {
            case Monster.MonsterState.Active:
                Proceeding = StartCoroutine("Patroll");
                break;
            case Monster.MonsterState.Chase:
                Proceeding = StartCoroutine("Chase");
                break;
            case Monster.MonsterState.Sleep:
                Proceeding = null;
                Sleep();
                break;
            case Monster.MonsterState.Hit:
                Proceeding = StartCoroutine("Hit");
                break;
            case Monster.MonsterState.Die:
                Die();
                break;
            default:
                break;
        }
    }

    protected override IEnumerator Chase()
    {
        _rigidbody.gravityScale = 0;
        float time = 6f;
        _animator.SetTrigger("Chase");
        _animator.SetBool("Move", true);

        float maxSpeed = 5f;
        while (time >= 0 && _monster.target)
        {
            if(_monster.status.speed < maxSpeed)
                _monster.status.speed += 0.0055f;
            _monster.direction = _monster.target.transform.position.x > transform.position.x ? 1 : -1;
            Vector3 moveDir = (_monster.target.transform.position - transform.position + Vector3.up * 0.5f).normalized;

            _rigidbody.velocity = moveDir * _monster.status.speed;
            transform.localScale = new Vector3(_monster.direction * Mathf.Abs(transform.localScale.x), 1f, 1f);

            time -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        if (FindEnemy(transform.position, 5f, _monster.target))
            StateChanged(Monster.MonsterState.Chase);
        else
        {
            _monster.status.speed = _monster.status.basicSpeed;
            StateChanged(Monster.MonsterState.Active);
        }
    }

    protected override IEnumerator Hit()
    {
        _animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.7f);
        StateChanged(Monster.MonsterState.Chase);
    }
    private bool FindEnemy(Vector3 position, float radius, GameObject enemy)
    {
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(position, radius);
        bool find = false;
        foreach (Collider2D collider in colliders2)
        {
            if (collider.gameObject.Equals(enemy))
            {
                find = true;
                break;
            }
        }
        return find;
    }
    protected override IEnumerator Patroll()
    {
        _monster.target = null;
        _rigidbody.gravityScale = 4;
        _animator.SetBool("Move", false);

        // Seek
        while (!_monster.target)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
            foreach(Collider2D collider in colliders)
            {
                if (_monster._attack.attackTargetTag.Contains(collider.tag))
                {
                    _monster.target = collider.gameObject;
                    _animator.SetBool("Move", true);
                    break;
                }
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        // Seek Capture
        if (_monster.target)
        {
            // Seen
            _animator.SetTrigger("Find");
            _monster.direction = _monster.target.transform.position.x > transform.position.x ? 1 : -1;
            transform.localScale = new Vector3(_monster.direction * Mathf.Abs(transform.localScale.x), 1f, 1f);

            yield return new WaitForSeconds(0.8f);

            // Find
            if (FindEnemy(transform.position, 5f, _monster.target)) 
                StateChanged(Monster.MonsterState.Chase);
            else
            {   // Cant Find
                _animator.SetBool("Move", false);
                StateChanged(Monster.MonsterState.Active);
            }
        }
    }
    //protected override void Die()
    //{
    //    _rigidbody.velocity = Vector3.zero;
    //    _rigidbody.gravityScale = 0;
    //    GetComponent<Collider2D>().enabled = false;
    //    _animator.SetTrigger("Hit");
    //    _monster.Die();
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (_monster._attack.attackTargetTag.Contains(collision.gameObject.tag))
    //    {
    //        _monster.target = touchingTarget = collision.gameObject;
    //        StateChanged(Monster.MonsterState.Chase);
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject == touchingTarget)
    //        touchingTarget = null;
    //}
}
