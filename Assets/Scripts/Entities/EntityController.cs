using JetBrains.Annotations;
using UnityEngine;

namespace Entities
{
    public class EntityController : MonoBehaviour
    {
        public Entity entity;

        public EntityStatus status;

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
    }
}