using System.Collections;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public abstract class MonsterController : MonoBehaviour
    {
        protected Rigidbody2D _rigidbody;
        protected Animator _animator;
        protected Monster _monster;
        protected EntityController _entityController;

        protected GameObject touchingTarget;

        [SerializeField] protected float chaseTime = 6f;


        public abstract void StateChanged(Monster.MonsterState state);
        protected abstract IEnumerator Chase();
        protected abstract IEnumerator Patroll();
        protected virtual void Sleep() { }
        protected abstract IEnumerator Hit();
        protected virtual void Die() {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.gravityScale = 0;
            _rigidbody.Sleep();
            _animator.SetTrigger("Hit");
            _monster.Die();
        }

        protected virtual void OnHit(EntityDamagedEventArgs args)
        {
            if (args.attacker == null) return;
            if (_entityController.isDead) return;
 
            _monster.target = args.attacker.gameObject;
            StateChanged(Monster.MonsterState.Hit);
        }

        protected virtual void OnDie(EntityDiedEventArgs args)
        {
            StateChanged(Monster.MonsterState.Die);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _monster = GetComponent<Monster>();
            _entityController = GetComponent<EntityController>();
                
        }

        private void OnEnable()
        {
            _entityController.damaged += OnHit;
            _entityController.died += OnDie;
        }

        private void OnDisable()
        {
            _entityController.damaged -= OnHit;
            _entityController.died -= OnDie;
        }

    }
}
