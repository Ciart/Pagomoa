using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityController))]
    public class EnemyContactDamage : MonoBehaviour
    {
        public float damage = 10f;

        private EntityController _entity;

        private List<EntityController> _targets = new List<EntityController>();

        private void Awake()
        {
            _entity = GetComponent<EntityController>();
        }

        private void Update()
        {
            if (!_entity.isEnemy)
            {
                return;
            }

            foreach (var enemy in _targets)
            {
                enemy.TakeDamage(damage, attacker: gameObject, flag: DamageFlag.Contact);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var entity = collision.attachedRigidbody.GetComponent<EntityController>();

            if (entity is null || _targets.Contains(entity) || entity.isEnemy)
            {
                return;
            }

            _targets.Add(entity);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var entity = collision.attachedRigidbody.GetComponent<EntityController>();

            if (entity is null || !_targets.Contains(entity))
            {
                return;
            }

            _targets.Remove(entity);
        }
    }
}