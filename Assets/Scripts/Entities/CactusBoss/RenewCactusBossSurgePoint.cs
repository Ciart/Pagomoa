using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossSurgePoint : MonoBehaviour
    {
        private RenewCactusBoss _renewCactusBoss;

        private void Start()
        {
            _renewCactusBoss = GetComponentInParent<RenewCactusBoss>();
        }

        void Update()
        {
            Vector2 point1 = new Vector2(_renewCactusBoss.points[0].transform.position.x,
                _renewCactusBoss.points[0].transform.position.y);
            Vector2 point2 = new Vector2(_renewCactusBoss.points[1].transform.position.x,
                _renewCactusBoss.points[1].transform.position.y);
            
            if (Physics2D.OverlapPoint(point1))
            {
                _renewCactusBoss.surgePoint = true;
            }
            if (!Physics2D.OverlapPoint(point2))
            {
                _renewCactusBoss.surgePoint = false;
            }
        }
    }
}
