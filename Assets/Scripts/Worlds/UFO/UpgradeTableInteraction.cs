using UnityEngine;

namespace Ciart.Pagomoa.Worlds.UFO
{
    public class UpgradeTableInteraction : MonoBehaviour
    {
        public GameObject effect;
        
        private SpriteRenderer _drillRenderer;

        void Start()
        {
            _drillRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
            if (!_drillRenderer) return ;

            _drillRenderer.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == "Player")
            {
                _drillRenderer.enabled = true;
                GameObject activatedEffect = Instantiate(effect, transform.position, Quaternion.identity);
                AnimatingOnce(activatedEffect);
            } 
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.name == "Player")
            {
                _drillRenderer.enabled = false;
                GameObject activatedEffect = Instantiate(effect, transform.position, Quaternion.identity);
                AnimatingOnce(activatedEffect);
            }
        }

        private void AnimatingOnce( GameObject activatedEffect )
        {
            Destroy(activatedEffect, 0.5f);
        }
    }
}