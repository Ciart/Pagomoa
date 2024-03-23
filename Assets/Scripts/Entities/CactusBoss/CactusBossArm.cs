using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossArm : MonoBehaviour
    {
        public float damage = 20f;

        private EntityController _controller;

        private List<EntityController> _targets = new List<EntityController>();

        private void Awake() {
            _controller = GetComponent<EntityController>();
        }

        private void OnEnable() {
            _controller.damaged += OnDamaged;
        }

        private void OnDisable() {
            _controller.damaged -= OnDamaged;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var entityController = other.GetComponent<EntityController>();

            if (entityController)
            {
                _targets.Add(entityController);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var entityController = other.GetComponent<EntityController>();

            if (entityController)
            {
                _targets.Remove(entityController);
            }
        }

        // TODO: 대미지를 상위 엔티티에게 전달하는 함수를 따로 분리해야 함.
        private void OnDamaged(EntityDamagedEventArgs args)
        {
            _controller.parent.TakeDamage(args.amount, attacker: args.attacker, flag: args.flag);
        }

        private void OnTakeDamage()
        {
            foreach (var target in _targets)
            {
                target.TakeDamage(damage, attacker: _controller);
            }
        }

        private void OnEnd() {
            _controller.parent.GetComponent<CactusBoss>().ResetArm();
            Destroy(gameObject);
        }
    }
}
