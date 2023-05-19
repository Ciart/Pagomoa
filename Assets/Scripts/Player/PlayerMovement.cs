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
        
        private static readonly int DirectionX = Animator.StringToHash("directionX");
        private static readonly int DirectionY = Animator.StringToHash("directionY");
        private static readonly int VelocityX = Animator.StringToHash("velocityX");
        private static readonly int VelocityY = Animator.StringToHash("velocityY");
        private static readonly int Speed = Animator.StringToHash("speed");
        private static readonly int IsClimb = Animator.StringToHash("isClimb");

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
            var velocity = direction.normalized * (climbSpeed * Time.deltaTime);

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

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            _animator.SetFloat(DirectionX, direction.x);
            _animator.SetFloat(DirectionY, direction.y);
            _animator.SetFloat(VelocityX, Mathf.Abs(_rigidbody.velocity.x));
            _animator.SetFloat(VelocityY, _rigidbody.velocity.y);
            _animator.SetFloat(Speed, _rigidbody.velocity.magnitude / 5f);
            _animator.SetBool(IsClimb, isClimb);
        }
    }
}
