using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public bool isClimb;

        public float gravityScale = 2.5f;

        public float speed = 250f;
        
        public float climbSpeed = 250f;
        
        public float jumpForce = 650f;
        
        public Vector2 direction = Vector2.zero;
        
        private Rigidbody2D _rigidbody;

        private Animator _animator;

        private MapManager _map;
        
        private bool _isJump;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _map = GetComponent<MapManager>();
        }

        public void Jump()
        {
            _isJump = true;
        }
        
        private void UpdateClimb()
        {
            var velocity = direction.normalized * climbSpeed * Time.deltaTime;

            // PlayerClimbingAnimation();

            _rigidbody.velocity = velocity;
            _rigidbody.gravityScale = 0;
        }

        private void UpdateWalk()
        {
            var velocity = new Vector2(direction.x * speed * Time.deltaTime, _rigidbody.velocity.y);

            _rigidbody.velocity = velocity;
            _rigidbody.gravityScale = gravityScale;
        }

        private void FixedUpdate()
        {
            if (_isJump)
            {
                _rigidbody.AddForce(new Vector2(0, jumpForce));
                
                _isJump = false;
            }
            
            if (isClimb)
            {
                UpdateClimb();
            }
            else
            {
                UpdateWalk();
            }
        }
    }
}
