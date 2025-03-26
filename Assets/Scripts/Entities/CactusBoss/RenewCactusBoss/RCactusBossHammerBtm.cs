using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class RCactusBossHammerBtm : MonoBehaviour
    {
        private bool _triggerEnter = false;
        public float damage = 10f;
        
        public EntityController _player;
        
        private void Start()
        {
            _player = GameManager.instance.player.GetComponent<EntityController>();
        }
        private void Update()
        {
            if(_triggerEnter)
                TakeDamage(damage, attacker: this.gameObject);
        }
        
        private void TakeDamage(float amount, GameObject attacker)
        {
            // _player.status.health -= amount;
            TakeKnockback(5f, attacker.transform.position - transform.position);
        }
        
        public void TakeKnockback(float force, Vector2 direction)
        {
            ParticleManager.instance.Make(0, _player.gameObject, Vector2.zero, 0.5f);

            _player.GetComponent<Rigidbody2D>().AddForce(force * direction.normalized, ForceMode2D.Impulse);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != _player.gameObject) return;

            _triggerEnter = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject != _player.gameObject) return;

            _triggerEnter = false;
        }
    }
}
