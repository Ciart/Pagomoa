using System;
using System.Collections;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    public class EntityDiedEventArgs
    {
        /// <summary>
        /// false일 경우 수동으로 제거해야 함
        /// default: true
        /// </summary>
        public bool AutoDespawn { get; set; } = true;
    }

    public class EntityController : MonoBehaviour
    {
        public string entityId;

        public float health;
        
        public float maxHealth;

        public float attack;
        
        public float defense;
        
        public float speed;

        public EntityController parent;

        public event Action<EntityDamagedEventArgs> damaged;

        public event Action<EntityExplodedEventArgs> exploded;

        public event Action<EntityDiedEventArgs> died;
        
        public bool isDead => health <= 0;

        private SpriteRenderer _spriteRenderer;

        private Rigidbody2D _rigidbody;

        private float _invincibleTime;

        public bool isEnemy
        {
            get;
            private set;
        }

        public bool isInvincible
        {
            get;
            private set;
        }

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
            var entity = ResourceSystem.instance.GetEntity(data.id);
            entityId = entity.id;
            isEnemy = entity.isEnemy;
            isInvincible = entity.isInvincible;

            if (data.status != null)
            {
                health = data.status.health;
                maxHealth = data.status.maxHealth;
                attack = data.status.attack;
                defense = data.status.defense;
                speed = data.status.speed;
                return;
            }
            
            health = entity.baseHealth;
            maxHealth = entity.baseHealth;
            attack = entity.attack;
            defense = entity.defense;
            speed = entity.speed;
        }
        
        public EntityData GetEntityData()
        {
            if (this == null) return null;

            var position = transform.position;
            
            var status = new EntityStatus
            {
                health = health,
                maxHealth = maxHealth,
                attack = attack,
                defense = defense,
                speed = speed
            };
            
            return new EntityData(entityId, position.x, position.y, status);
        }

        /*private void OnDrawGizmos()
        {
            Handles.Label(transform.position, $"({status.health} / {status.maxHealth})");
        }*/

        public void TakeKnockback(float force, Vector2 direction)
        {
            if(isDead) return;

            ParticleManager.instance.Make(0, gameObject, Vector2.zero, 0.5f);

            _rigidbody.AddForce(force * direction.normalized, ForceMode2D.Impulse);
        }

        public void TakeDamage(float amount, float invincibleTime = 0f, EntityController attacker = null,
            DamageFlag flag = DamageFlag.None)
        {
            if (isInvincible || isDead)
            {
                return;
            }

            _invincibleTime = invincibleTime;

            var damage = amount * (1 - defense / (defense + 100));

            health -= damage;
            damaged?.Invoke(new EntityDamagedEventArgs { amount = damage, invincibleTime = invincibleTime, attacker = attacker, flag = flag });

            if (health <= 0)
            {
                health = 0;

                // TODO: preDied 이벤트 추가
                Die();
                return;
            }

            if (attacker is not null)
            {
                TakeKnockback(5f, transform.position - attacker.transform.position);
            }

            StartCoroutine(RunInvincibleTimeFlash());
        }

        public void TakeExploded(float amount)
        {
            if (!isInvincible)
            {
                health -= amount;
            }
            exploded?.Invoke(new EntityExplodedEventArgs { amount = amount });
        }

        public void Die()
        {
            health = 0;
            
            var args = new EntityDiedEventArgs();
            
            died?.Invoke(args);
            
            if (args.AutoDespawn)
            {
                // TODO: EntityManager에서 관리해야 함
                Destroy(gameObject);
            }
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
            if (!isDead && health <= 0)
            {
                Die();
            }
        }

        private void Update()
        {
            CheckDeath();

            var distance = Vector3.Distance(transform.position, Game.instance.player.transform.position);

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
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                _isGrounded = false;
            }
        }
    }
}
