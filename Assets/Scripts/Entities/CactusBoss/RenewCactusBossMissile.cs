using System.Collections;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossMissile : MonoBehaviour
    {
        public float missileSpeed;
        public float trackStartTime;
        
        private Rigidbody2D _rigid;
        private GameObject _targetObject;
        void Start()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _targetObject = GameManager.player.gameObject;
            
            StartCoroutine(ShootMissile());
        }
        IEnumerator ShootMissile()
        {
            _rigid.AddForce(Vector2.up * missileSpeed);

            yield return new WaitForSeconds(trackStartTime);

            TrackPlayer();

        }

        private void TrackPlayer()
        {
            _rigid.velocity = Vector3.zero;
            
            Vector3 dir = (_targetObject.transform.position - this.transform.position).normalized;

            float vx = dir.x * missileSpeed / 10;
            float vy = dir.y * missileSpeed / 10;

            _rigid.velocity = new Vector2(vx, vy);
            this.GetComponent<SpriteRenderer>().flipX = (vx < 0);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == GameManager.player.gameObject)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
