using System.Collections.Generic;
using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Players
{
    public class DrillController : MonoBehaviour
    {
        public SpriteResolver head;

        public SpriteResolver body;

        public Vector2 upOffset;

        public Vector2 downOffset;

        public Vector2 leftOffset;

        public Vector2 rightOffset;

        private PlayerDigger _digger;

        private Direction _prevDirection;

        private List<EntityController> _enemies = new List<EntityController>();

        private void Awake()
        {
            _digger = transform.parent.GetComponent<PlayerDigger>();
        }

        private void UpdateDirection()
        {
            var direction = _digger.direction;

            if (direction == _prevDirection)
            {
                return;
            }

            _prevDirection = direction;

            transform.localPosition = direction switch
            {
                Direction.Up => upOffset,
                Direction.Down => downOffset,
                Direction.Left => leftOffset,
                Direction.Right => rightOffset,
                _ => downOffset
            };

            head.row = (int)direction;
            body.row = (int)direction;
        }

        private void Update()
        {
            UpdateDirection();
        }

        private void OnEnable()
        {
            TimeManager.instance.tickUpdated += OnTickUpdated;
        }

        private void OnDisable()
        {
            TimeManager.instance.tickUpdated -= OnTickUpdated;
        }

        private void OnTickUpdated(int tick)
        {
            foreach (var enemy in _enemies)
            {
                // TODO: attacker 변경해야 함.
                // TODO: 무적 시간을 빼고 다른 시각적 효과를 줘야 함.
                enemy.TakeDamage(5, invincibleTime: 0.3f, attacker: null, flag: DamageFlag.Melee);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var entity = collision.GetComponent<EntityController>();

            if (entity is null || _enemies.Contains(entity) || !entity.isEnemy)
            {
                return;
            }

            _enemies.Add(entity);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var entity = collision.GetComponent<EntityController>();

            if (entity is null || !_enemies.Contains(entity))
            {
                return;
            }

            _enemies.Remove(entity);
        }
    }
}
