using System;
using System.Collections;
using Ciart.Pagomoa.Entities.Players;
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

        private float _health; 
        public float health 
        {
            get => _health;
            set
            {
                _health = value >= maxHealth ? maxHealth : value;
                if (_health <= 0.0f) _health = 0.0f;
                healthChanged?.Invoke();
            }
        }
        
        private float _maxHealth;
        public float maxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                healthChanged?.Invoke();
            }
        }

        private float attack;
        public float Attack
        {
            get => attack;
            set
            {
                attack = value;
                attackChanged?.Invoke();
            }
        }
        public event Action attackChanged;

        private float deffense;
        public float Defense
        {
            get => deffense;
            set
            {
                deffense = value;
                deffenseChanged?.Invoke();
            }
        }
        public event Action deffenseChanged;


        private float speed;
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                speedChanged?.Invoke();
            }
        }
        public event Action speedChanged;

        public EntityController parent;

        public event Action healthChanged;
        public event Action<EntityDamagedEventArgs> damaged;
        public event Action<EntityExplodedEventArgs> exploded;
        public event Action<EntityDiedEventArgs> died;
        
        public bool isDead => health <= 0;

        protected SpriteRenderer _spriteRenderer;

        protected Rigidbody2D _rigidbody;

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

        public virtual void Init(EntityData data)
        {
            var entity = ResourceSystem.instance.GetEntity(data.id);
            entityId = entity.id;
            isEnemy = entity.isEnemy;
            isInvincible = entity.isInvincible;

            if (data.status != null)
            {
                health = data.status.health;
                maxHealth = data.status.maxHealth;
                Attack = data.status.attack;
                Defense = data.status.defense;
                Speed = data.status.speed;
                return;
            }
            
            health = entity.baseHealth;
            maxHealth = entity.baseHealth;
            Attack = entity.attack;
            Defense = entity.defense;
            Speed = entity.speed;
        }
        
        public EntityData GetEntityData()
        {
            if (this == null) return null;

            var position = transform.position;
            
            var status = new EntityStatus
            {
                health = health,
                maxHealth = maxHealth,
                attack = Attack,
                defense = Defense,
                speed = Speed
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

            Game.Instance.Particle.Make(0, gameObject, Vector2.zero, 0.5f);

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

            var damage = amount * (1 - Defense / (Defense + 100));

            health -= damage;
            damaged?.Invoke(new EntityDamagedEventArgs { amount = damage, invincibleTime = invincibleTime, attacker = attacker, flag = flag });
            healthChanged?.Invoke();
            
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

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
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

            var player = Game.Instance.player;
            
            if (player is null || this == player) return;

            var distance = Vector3.Distance(transform.position, player.transform.position);

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
