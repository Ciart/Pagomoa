using System.Collections;
using System.Drawing;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private LayerMask _damageTargetLayer;

        private SpriteRenderer _spriteRenderer;
        private IEnumerator corutine;

        private void Awake()
        {
            transform.GetChild(0).TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        }

        public void Init(float waitingTime, int boomRange)
        {
            corutine = Wait(waitingTime, boomRange);
            StartCoroutine(corutine);
        }
    
        private IEnumerator Wait(float waitTime, int boomRange)
        {
            var itemEntity = GetComponentInParent<ItemEntity>();
            var timer = 0f;
            while (timer < waitTime)
            {
                timer += Time.deltaTime;
                itemEntity.frequency = (waitTime - timer) * 5f;
                yield return null;
            }
            Boom(boomRange);
        }

        private void Boom(int boomRange)
        {
            var duration = 2f;
            _spriteRenderer.enabled = false;
            Game.Instance.Particle.Make(2, null, transform.position, duration);
            Destroy(gameObject, duration);
            Game.Instance.Sound.PlaySfx("BombEffect", true, this.transform.position);
            
            Earthquake(boomRange);
        }

        private void Earthquake(int bound)
        {
            var point = transform.position;
            var digPoint = point;

            for (int i = -bound; i <= bound; i++)
            {
                digPoint.x = point.x + i;
                for (int j = -bound; j <= bound; j++)
                {
                    digPoint.y = point.y + j;
                    var pointInt = WorldManager.ComputeCoords(digPoint);
                    Game.Instance.World.BreakGround(pointInt.x, pointInt.y, 99999, true);
                }
            }

             Collider2D[] colliders = Physics2D.OverlapAreaAll(point + new Vector3(-bound, -bound), point + new Vector3(bound, bound), _damageTargetLayer);
            foreach(Collider2D collider in colliders)
            {
                var entityController = collider.GetComponent<EntityController>();
                if (entityController) { entityController.TakeExploded(999); }
            }
        }

        //private void Boom(int bound)
        //{
        //    var point = _bombEffect.transform.position + new Vector3(-2, -2.2f);

        //    for (int j = 0; j < bound; j++)
        //    {
        //        point.y += 1;
        //        for (int i = 0; i < bound; i++)
        //        {
        //            point.x += 1;
        //            var pointInt = WorldManager.ComputeCoords(point);
        //            WorldManager.instance.BreakGround(pointInt.x, pointInt.y, 99999, true);
        //        }
        //        point.x = _bombEffect.transform.position.x - 2;
        //    }
        //}

        private void Destroy(GameObject Bomb, int time)
        {
            Destroy(Bomb, time);
        }
    }
}
