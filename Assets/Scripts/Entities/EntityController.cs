using System;
using System.Collections;
using Systems;
using UnityEditor;
using UnityEngine;

namespace Entities
{
    [Flags]
    public enum DamageFlag
    {
        None = 0,
        Contact = 1 << 0,
        Melee = 1 << 1,
    }

    public class EntityDamagedEventArgs
    {
        public float amount;
        public GameObject attacker;
        public DamageFlag flag;
    }

    public class EntityController : MonoBehaviour
    {
        public Entity entity;

        public EntityStatus status;

        public bool isEnemy = false;

        public event Action<EntityDamagedEventArgs> damaged;

        private SpriteRenderer _spriteRenderer;

        private Rigidbody2D _rigidbody;

        private float _invincibleTime;

        public bool isInvincibleTime => _invincibleTime > 0;

        public void Init(Entity entity, EntityStatus status = null)
        {
            this.entity = entity;

            if (status is null)
            {
                this.status = new EntityStatus
                {
                    health = entity.baseHealth,
                    maxHealth = entity.baseHealth
                };

                isEnemy = entity.isEnemy;
            }
            else
            {
                this.status = status;
            }
        }

        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, $"({status.health} / {status.maxHealth})");
        }

        public void TakeKnockback(float force, Vector2 direction)
        {
            ParticleManager.Instance.Make(0, gameObject, Vector2.zero, 0.5f);

            _rigidbody.AddForce(force * direction.normalized, ForceMode2D.Impulse);
        }

        public void TakeDamage(float amount, float invincibleTime = 0.3f, GameObject attacker = null,
            DamageFlag flag = DamageFlag.None)
        {
            if (isInvincibleTime)
            {
                return;
            }

            _invincibleTime = invincibleTime;
            
            status.health -= amount;
            if (status.health <= 0)
            {
                status.health = 0;
            }

            if (attacker is not null)
            {
                TakeKnockback(10f, transform.position - attacker.transform.position);
            }

            damaged?.Invoke(new EntityDamagedEventArgs { amount = amount, attacker = attacker, flag = flag });

            StartCoroutine(RunInvincibleTimeFlash());
        }

        private IEnumerator RunInvincibleTimeFlash()
        {
            while (isInvincibleTime)
            {
                _spriteRenderer.enabled = false;
                yield return new WaitForSeconds(0.05f);
                _spriteRenderer.enabled = true;
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.simulated = false;
        }
        
        private void CheckDeath()
        {
            if (status.health <= 0)
            {
                Die();
            }
        }

        private void Update()
        {
            CheckDeath();
            
            var distance = Vector3.Distance(transform.position, EntityManager.instance.player.transform.position);

            if (distance > 10f)
            {
                _rigidbody.simulated = false;
            }
            else
            {
                _rigidbody.simulated = true;
            }

            _invincibleTime -= Time.deltaTime;
        }
    }
}