using System.Collections.Generic;
using Constants;
using UnityEngine;

namespace Entities.Players
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
            
            foreach (var enemy in _enemies)
            {
                enemy.TakeDamage(10);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var entity = collision.GetComponent<EntityController>();

            if (entity == null || !entity.isEnemy || entity.gameObject == _digger.gameObject)
            {
                return;
            }

            if (!_enemies.Contains(entity))
            {
                _enemies.Add(entity);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var entity = collision.GetComponent<EntityController>();
            
            if (entity == null)
            {
                return;
            }

            _enemies.Remove(entity);
        }
    }
}