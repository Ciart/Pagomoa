using System;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBoss : MonoBehaviour
    {
        public EntityOrigin armOrigin;

        public float speed = 20f;

        public float attackRange = 30f;

        public float attackRate = 1f;

        [HideInInspector]
        [NonSerialized]
        public EntityController controller;

        [HideInInspector]
        [NonSerialized]
        public Rigidbody2D rigidbody;

        [HideInInspector]
        [NonSerialized]
        public SpriteRenderer spriteRenderer;

        private Animator _animator;

        private float _nextAttack;

        public bool CheckPlayerInRange() => Vector3.Distance(EntityManager.instance.player.transform.position, transform.position) <= attackRange;

        public bool CheckAttackAble() => Time.time > _nextAttack;

        private void Awake()
        {
            controller = GetComponent<EntityController>();
            rigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        // TODO: 삭제 예정.
        public void ResetArm()
        {
            _animator.SetTrigger("Idle");
        }

        public void ApplyAttackRate()
        {
            _nextAttack = Time.time + attackRate;
        }

        private void OnSpawnArm()
        {
            var player = EntityManager.instance.player;
            var spawnPosition = new Vector2(player.transform.position.x, 0);

            var arm = EntityManager.instance.Spawn(armOrigin, spawnPosition);
            arm.parent = controller;
        }
    }
}