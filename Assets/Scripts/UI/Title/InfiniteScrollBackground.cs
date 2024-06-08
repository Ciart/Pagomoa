using UnityEngine;

namespace Ciart.Pagomoa.UI.Title
{
    public class InfiniteScrollBackground : ScrollBackground
    {
        public override void Scroll()
        {
            transform.position += Time.deltaTime * moveDirection * speed;
            if (transform.position.y >= 32.5f)
            {
                gameObject.transform.position = new Vector2(0,-50.5f);
            }
        }
    }
}
