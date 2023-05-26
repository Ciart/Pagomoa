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
      
        // TODO: 최대 거리 검사가 필요 함.
        public Vector2 directionVector = Vector2.zero;

        private static readonly int AnimatorDirectionX = Animator.StringToHash("directionX");
        private static readonly int AnimatorDirectionY = Animator.StringToHash("directionY");
        private static readonly int AnimatorVelocityX = Animator.StringToHash("velocityX");
        private static readonly int AnimatorVelocityY = Animator.StringToHash("velocityY");
        private static readonly int AnimatorSpeed = Animator.StringToHash("speed");
        private static readonly int AnimatorIsClimb = Animator.StringToHash("isClimb");

        private Rigidbody2D _rigidbody;

        private Animator _animator;

        private MapManager _map;

        private bool _isJump;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _map = MapManager.Instance;
        }

        public void Jump()
        {
            _isJump = true;
        }

        private void UpdateClimb()
        {
            var velocity = directionVector * (climbSpeed * Time.deltaTime);

            var a = transform.position + new Vector3(0f, 1f, 0f);

            if (!_map.CheckClimbable(a))
            {
                velocity = new Vector2(velocity.x, Mathf.Min(velocity.y, a.y));
            }

            _rigidbody.velocity = velocity;
            _rigidbody.gravityScale = 0;
        }

        private void UpdateWalk()
        {
            var velocity = new Vector2(directionVector.x * speed * Time.deltaTime, _rigidbody.velocity.y);

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
            _animator.SetFloat(AnimatorDirectionX, directionVector.x > 0.0001 ? 1 : 0);
            _animator.SetFloat(AnimatorDirectionY, directionVector.y > 0.0001 ? 1 : 0);
            _animator.SetFloat(AnimatorVelocityX, Mathf.Abs(_rigidbody.velocity.x));
            _animator.SetFloat(AnimatorVelocityY, _rigidbody.velocity.y);
            _animator.SetFloat(AnimatorSpeed, _rigidbody.velocity.magnitude / 5f);
            _animator.SetBool(AnimatorIsClimb, isClimb);
        }
    }
}