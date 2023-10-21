using System;
using JetBrains.Annotations;
using UnityEngine;
using Worlds;

namespace Entities
{
    public class EntityController : MonoBehaviour
    {
        public Entity entity;

        public EntityStatus status;

        private Rigidbody2D _rigidbody;

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
            }
            else
            {
                this.status = status;
            }
        }

        public void TakeDamage(float damage, float InvincibleTime = 0.3f, GameObject attacker = null)
        {
            status.health -= damage;

            if (status.health <= 0)
            {
                status.health = 0;
                Die();
            }
        }

        private void Die()
        {
            //
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.simulated = false;
        }

        private void Update()
        {
            var distance = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);

            if (distance > 10f)
            {
                _rigidbody.simulated = false;
            }
            else
            {
                _rigidbody.simulated = true;
            }
        }
    }
}