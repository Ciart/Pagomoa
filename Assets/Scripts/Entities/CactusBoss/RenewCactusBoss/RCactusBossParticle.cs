using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class CactusBossParticle : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                Debug.Log("Player entered");
                Destroy(gameObject);
            }
        }
    }
}
