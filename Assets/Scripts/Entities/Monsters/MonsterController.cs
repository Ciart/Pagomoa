using System.Collections;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public abstract class MonsterController : MonoBehaviour
    {
        protected Rigidbody2D _rigidbody;
        protected Animator _animator;
        protected Monster _monster;

        protected GameObject touchingTarget;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _monster = GetComponent<Monster>();
        }

        public abstract void StateChanged(Monster.MonsterState state);
        protected abstract IEnumerator Chase();
        protected abstract IEnumerator Patroll();
        protected virtual void Sleep() { }
        protected abstract IEnumerator Hit();
        protected virtual void Die() {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.gravityScale = 0;
            GetComponent<Collider2D>().enabled = false;
            _animator.SetTrigger("Hit");
            _monster.Die();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // if (_monster._attack.attackTargetTag.Contains(collision.gameObject.tag))
            // {
            //     _monster.target = touchingTarget = collision.gameObject;
            //     StateChanged(Monster.MonsterState.Chase);
            // }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject == touchingTarget)
                touchingTarget = null;
        }

    }
}
