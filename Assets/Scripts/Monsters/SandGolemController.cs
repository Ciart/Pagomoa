using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Monsters;
using Ciart.Pagomoa.Systems.Time;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class SandGolemController : MonsterController
    {
        Coroutine Proceeding;

        public LayerMask enemyLayer;

        void FixedUpdate()
        {
            if (touchingTarget)
            {
                switch (touchingTarget.tag)
                {
                    case "Player":
                        // _monster._attack._Attack(gameObject, touchingTarget, _monster.status.attackPower);
                        break;
                }
            }
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
                case Monster.MonsterState.Attack:
                    Proceeding = StartCoroutine("Attack");
                    break;
                case Monster.MonsterState.Hit:
                    Proceeding = StartCoroutine("Hit");
                    break;
                case Monster.MonsterState.Die:
                    StopAllCoroutines();
                    Die();
                    break;
                default:
                    break;
            }
        }

        protected override IEnumerator Patroll()
        {
            float time = 20f;
            for (int changeDir = 6; changeDir > 0; changeDir--)
            {
                _monster.direction = Random.Range(-1, 2);
                if (_monster.direction != 0)
                    _animator.SetTrigger("Patroll");
                else
                    _animator.SetTrigger("Idle");
                while (time >= 3 * changeDir)
                {
                    _rigidbody.linearVelocity = new Vector2(_monster.direction * _monster.status.speed, _rigidbody.linearVelocity.y);
                    if (_monster.direction != 0)
                        transform.localScale = new Vector3(_monster.direction * Mathf.Abs(transform.localScale.x), 1f, 1f);

                    time -= Time.fixedDeltaTime;
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }
            }
        }

        protected override IEnumerator Chase()
        {
            float time = chaseTime;
            _animator.SetTrigger("Chase");
            while (time >= 0 && _monster.target)
            {
                _monster.direction = _monster.target.transform.position.x > transform.position.x ? 1 : -1;

                _rigidbody.linearVelocity = new Vector2(_monster.direction * _monster.status.speed, _rigidbody.linearVelocity.y);
                transform.localScale = new Vector3(_monster.direction * Mathf.Abs(transform.localScale.x), 1f, 1f);

                CheckEnemyInAttackRange();

                time -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            StateChanged(Monster.MonsterState.Active);
        }

        protected override void Sleep()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _animator.SetTrigger("Sleep");
        }

        protected IEnumerator Attack()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.15f);
            AttackEnemy();
            yield return new WaitForSeconds(0.45f);
            StateChanged(Monster.MonsterState.Chase);
        }

        protected override IEnumerator Hit()
        {
            _animator.SetTrigger("Hit");

            yield return new WaitForSeconds(0.7f);
            StateChanged(Monster.MonsterState.Chase);
        }

        private void CheckEnemyInAttackRange()
        {
            var colliders = Physics2D.OverlapAreaAll(transform.position, transform.position + new Vector3(_monster.direction, 0.2f), enemyLayer);
            foreach(var collider in colliders)
            {
                var entity = collider.GetComponent<EntityController>();
                if (entity == GetComponent<EntityController>()) continue;

                if (entity is not null)
                {
                    StateChanged(Monster.MonsterState.Attack);
                    return;
                }
            }
        }

        private void AttackEnemy()
        {
            var colliders = Physics2D.OverlapAreaAll(transform.position, transform.position + new Vector3(_monster.direction, 0.2f), enemyLayer);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<EntityController>();
                if (entity == GetComponent<EntityController>()) continue;
                if (entity is null) continue;

                entity.TakeDamage(5, invincibleTime: 0.3f, attacker: GetComponentInParent<EntityController>(), flag: DamageFlag.Melee);
            }
        }

    }
}

