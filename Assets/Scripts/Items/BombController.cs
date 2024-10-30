using System.Collections;
using System.Drawing;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    public class BombController : MonoBehaviour
    {
        [SerializeField] private float _waitingTime;
        [SerializeField] private int _boomRange;
        [SerializeField] private GameObject _bombImage;
        [SerializeField] private GameObject _bombEffect;
        [SerializeField] private LayerMask _damageTargetLayer;

        private IEnumerator corutine;
        void Start()
        {
            corutine = Wait(_waitingTime, gameObject);
            StartCoroutine(corutine);
        }
    
        private IEnumerator Wait(float WaitTime, GameObject Bomb)
        {
            yield return new WaitForSeconds(WaitTime);
            SetBomb(Bomb);
            Destroy(Bomb, _waitingTime);
        }

        private void SetBomb(GameObject Bomb)
        {
            _bombImage.SetActive(false);
            _bombEffect.transform.localScale = new Vector3(10, 10, 1);
            _bombEffect.SetActive(true);
            _bombEffect.transform.position = _bombImage.transform.position;

            Boom(_boomRange);
            
            SoundManager.instance.PlaySfx("BombEffect", true, this.transform.position);
        }

        private void Boom(int bound)
        {
            var point = _bombEffect.transform.position;
            var digPoint = point;

            for (int i = -bound; i <= bound; i++)
            {
                digPoint.x = point.x + i;
                for (int j = -bound; j <= bound; j++)
                {
                    digPoint.y = point.y + j;
                    var pointInt = WorldManager.ComputeCoords(digPoint);
                    WorldManager.instance.BreakGround(pointInt.x, pointInt.y, 99999, true);
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
