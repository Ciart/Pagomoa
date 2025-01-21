using System;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossShockWave : MonoBehaviour
    {
        [SerializeField] private bool isRight;        
        private Rigidbody2D _rigidbody;
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        public void AddForce(float speed, float dir)
        {
            _rigidbody.AddForce(new Vector2(dir, 0) * speed);
        }
    }
}
