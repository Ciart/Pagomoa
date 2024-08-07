using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.UI.Title
{
    public class ScrollBackground : MonoBehaviour
    {
        [Range(1f, 150f)] public float speed;
        public Vector3 moveDirection = Vector3.up;
        
        private void Update()
        {
            Scroll();
        }

        protected virtual void Scroll() { }
    }
}
