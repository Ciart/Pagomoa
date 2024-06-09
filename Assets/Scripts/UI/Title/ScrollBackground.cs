using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.UI.Title
{
    public class ScrollBackground : MonoBehaviour
    {
        [Range(1f, 20f)] public float speed;
        public Vector3 moveDirection = Vector3.up;
        private void Update()
        {
            Scroll();
        }
        
        public virtual void Scroll()
        {
            transform.position += Time.deltaTime * moveDirection * speed;
            if (transform.position.y >= 39.35f)
            {
                Destroy(gameObject);
            }
        }
    }
}
