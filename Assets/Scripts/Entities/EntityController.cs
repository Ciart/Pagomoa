using System;
using System.Collections;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Ciart.Pagomoa.Entities
{
    [Flags]
    public enum DamageFlag
    {
        None = 0,
        Contact = 1 << 0, // 접촉
        Melee = 1 << 1, // 근접
    }

    // TODO: 무적 시간 추가
    public class EntityDamagedEventArgs
    {
        public float amount;
        public float invincibleTime;
        public EntityController attacker;
        public DamageFlag flag;
    }

    public class EntityExplodedEventArgs
    {
        public float amount;
    }

    public class EntityController : MonoBehaviour
    {
        public EntityOrigin origin;

        public EntityStatus status;

        public EntityController parent;

        public event Action<EntityDamagedEventArgs> damaged;

        public event Action<EntityExplodedEventArgs> exploded;

        public event Action died;
        
        public bool isDead => status.health <= 0;

        private SpriteRenderer _spriteRenderer;

        private Rigidbody2D _rigidbody;

        private float _invincibleTime;

        public bool isEnemy => origin.isEnemy;

        public bool isInvincible => _invincibleTime > 0 || origin.isInvincible;

        // https://discussions.unity.com/t/how-do-i-check-if-my-rigidbody-player-is-grounded/33250/11
        private bool _isGrounded;

        public bool isGrounded
        {
            get
            {
                if (_isGrounded)
                {
                    _isGrounded = false;
                    return true;
                }

                return false;
            }
        }

        public void Init(EntityData data)
        {
            origin = data.origin;

            // if (status is null)
            {
                status = new EntityStatus
                {
                    health = origin.baseHealth,
                    maxHealth = origin.baseHealth
                };
            }
            // else
            // {
            //     status = data.status;
            // }
        }
        
        public EntityData GetEntityData()
        {
            var position = transform.position;
            
            return new EntityData(position.x, position.y, origin, status);
        }

        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, $"({status.health} / {status.maxHealth})");
        }

        public void TakeKnockback(float force, Vector2 direction)
        {
            ParticleManager.instance.Make(0, gameObject, Vector2.zero, 0.5f);

            _rigidbody.AddForce(force * direction.normalized, ForceMode2D.Impulse);
        }

        public void TakeDamage(float amount, float invincibleTime = 0f, EntityController attacker = null,
            DamageFlag flag = DamageFlag.None)
        {
            if (isInvincible)
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
                TakeKnockback(5f, transform.position - attacker.transform.position);
            }

            damaged?.Invoke(new EntityDamagedEventArgs { amount = amount, invincibleTime = invincibleTime, attacker = attacker, flag = flag });
            StartCoroutine(RunInvincibleTimeFlash());
        }

        public void TakeExploded(float amount)
        {
            if (!isInvincible)
            {
                status.health -= amount;
            }
            exploded?.Invoke(new EntityExplodedEventArgs { amount = amount });
        }

        public void Die()
        {
            status.health = 0;
            gameObject.SetActive(false);
            died?.Invoke();
        }

        private IEnumerator RunInvincibleTimeFlash()
        {
            while (isInvincible)
            {
                _spriteRenderer.color = Color.clear;
                yield return new WaitForSeconds(0.05f);
                _spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.simulated = false;
        }

        private void CheckDeath()
        {
            if (!isDead && status.health <= 0)
            {
                Die();
            }
        }

        private void Update()
        {
            CheckDeath();

            var distance = Vector3.Distance(transform.position, GameManager.player.transform.position);

            if (distance > 100f)
            {
                _rigidbody.simulated = false;
            }
            else
            {
                _rigidbody.simulated = true;
            }

            _invincibleTime -= Time.deltaTime;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                _isGrounded = true;
                return;
            }

            _isGrounded = false;
            return;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                _isGrounded = false;
                return;
            }
        }
    }
}
