using System;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossShockWave : MonoBehaviour
    {
        public float speed;
        public float dir;
        
        Rigidbody2D _rigidbody;
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        { 
            _rigidbody.AddForce(new Vector2(dir, 0) * speed);
        }
    }
}
