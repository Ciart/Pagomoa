using System;
using System.Collections;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public class SnakeController : MonsterController
    {
        private Coroutine _proceeding;
        private Monster.MonsterState _curState;
        private RaycastHit2D[] _raycastResults = new RaycastHit2D[5];
        public LayerMask enemyLayer;
        
        private float _chaseTimer = 0f;
        
        private void FixedUpdate()
        {
            if (_curState == Monster.MonsterState.Sleep) return;
            if (_curState == Monster.MonsterState.Chase) return;
                CheckEnemyInChaseRange();
        }

        public override void StateChanged(Monster.MonsterState state)
        {
            if (_proceeding != null)
                StopCoroutine(_proceeding);
            _curState = state;
            
            switch (_curState)
            {
                case Monster.MonsterState.Active:
                    _proceeding = StartCoroutine(nameof(Patroll));
                    break;
                case Monster.MonsterState.Chase:
                    _proceeding = StartCoroutine(nameof(Chase));
                    break;
                case Monster.MonsterState.Sleep:
                    _proceeding = null;
                    Sleep();
                    break;
                case Monster.MonsterState.Attack:
                    _proceeding = StartCoroutine(nameof(Attack));
                    break;
                case Monster.MonsterState.Hit:
                    _proceeding = StartCoroutine(nameof(Hit));
                    break;
                case Monster.MonsterState.Die:
                    StopAllCoroutines();
                    Die();
                    break;
            }
        }

        protected override void Sleep()
        {
            _rigidbody.linearVelocity = Vector2.zero;
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
        
        protected override IEnumerator Chase()
        {
            _chaseTimer = chaseTime;
            _animator.SetTrigger("Move");

            while (_monster.target != null)
            {
                _chaseTimer -= Time.fixedDeltaTime;
                if (_chaseTimer <= 0f)
                {
                    _monster.target = null;
                    StateChanged(Monster.MonsterState.Active);
                    yield break;
                }

                var direction = _monster.target.transform.position.x > transform.position.x ? 1 : -1;

                _monster.direction = direction;
                _rigidbody.linearVelocity = new Vector2(direction * _monster.status.speed, _rigidbody.linearVelocity.y);
                transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), 1f, 1f);

                CheckEnemyInAttackRange();

                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            StateChanged(Monster.MonsterState.Active); // fallback if target lost
        }


        protected override IEnumerator Patroll()
        {
            float time = 20f;
            for (int changeDir = 6; changeDir > 0; changeDir--)
            {
                _monster.direction = Random.Range(-1, 2);
                if (_monster.direction != 0)
                    _animator.SetTrigger("Move");
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

        protected override IEnumerator Hit()
        {
            _animator.SetTrigger("Hit");

            yield return new WaitForSeconds(0.7f);
            StateChanged(Monster.MonsterState.Chase);
        }

        private void CheckEnemyInChaseRange()
        {
            Vector2 origin = transform.position;
            Vector2[] directions = { Vector2.left, Vector2.right };

            foreach (var dir in directions)
            {
                int hitCount = Physics2D.RaycastNonAlloc(origin, dir, _raycastResults, 3.0f, enemyLayer);
                for (int i = 0; i < hitCount; i++)
                {
                    var hit = _raycastResults[i];
                    if (hit.collider == null) continue;

                    var entity = hit.collider.GetComponent<EntityController>();
                    if (entity == null || entity == _entityController) continue;

                    _monster.target = entity.gameObject;
                    _monster.direction = dir == Vector2.right ? 1 : -1;
                    StateChanged(Monster.MonsterState.Chase);
                    return;
                }
            }
        }

        
        private void CheckEnemyInAttackRange()
        {
            var colliders = Physics2D.OverlapAreaAll(transform.position,
                transform.position + new Vector3(_monster.direction, 0.5f), enemyLayer);
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
                
                _chaseTimer = chaseTime;
                break;
            }
        }

    }
}
