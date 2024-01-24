using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Systems;
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

        private void Awake()
        {
            _digger = transform.parent.GetComponent<PlayerDigger>();
        }

        private void Update()
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
    }
}