using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public bool canMove = true;

        public bool isClimb;

        public bool isSideWall;

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
        private static readonly int AnimatorIsSideWall = Animator.StringToHash("isSideWall");
        private static readonly int AnimatorEndClimb = Animator.StringToHash("endClimb");

        private Rigidbody2D _rigidbody;

        private Animator _animator;

        private WorldManager _world;

        private bool _isJump;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _world = WorldManager.instance;
        }

        public void Jump()
        {
            _isJump = true;
        }
        
        private void UpdateClimb()
        {
            if (_animator.GetBool(AnimatorEndClimb)) return;

            var velocity = directionVector * (climbSpeed * Time.deltaTime);
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("endClimb"))
                _animator.SetBool(AnimatorEndClimb, false);

            
            if(velocity.y > 0 && CheckClimbEnable())
            {
                velocity.y = 0;

                if (isSideWall && !_animator.GetCurrentAnimatorStateInfo(0).IsName("endClimb"))
                    _animator.SetBool(AnimatorEndClimb, true);
            }
            _rigidbody.linearVelocity = velocity;
            _rigidbody.gravityScale = 0;
        }
        void EndClimbLeft()
        {
            Vector3 movePos = new Vector3(-0.4f, 1.04f);
            transform.position += movePos;
            _animator.SetBool(AnimatorEndClimb, false);
        }
        void EndClimbRight()
        {
            Vector3 movePos = new Vector3(0.4f, 1.04f);
            transform.position += movePos;
            _animator.SetBool(AnimatorEndClimb, false);
        }
        private void UpdateWalk()
        {
            if (_animator.GetBool(AnimatorEndClimb)) return;

            var velocity = new Vector2(directionVector.x * speed * Time.deltaTime, _rigidbody.linearVelocity.y);

            _rigidbody.linearVelocity = velocity;
            _rigidbody.gravityScale = gravityScale;
        }

        private void FixedUpdate()
        {
            CheckClimbEnable();
            if (!canMove)
            {
                return;
            }

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
            _animator.SetFloat(AnimatorDirectionX, directionVector.x);
            _animator.SetFloat(AnimatorDirectionY, directionVector.y);
            _animator.SetFloat(AnimatorVelocityX, Mathf.Abs(_rigidbody.linearVelocity.x));
            _animator.SetFloat(AnimatorVelocityY, _rigidbody.linearVelocity.y);
            _animator.SetFloat(AnimatorSpeed, _rigidbody.linearVelocity.magnitude / 5f);
            _animator.SetBool(AnimatorIsClimb, isClimb);
            _animator.SetBool(AnimatorIsSideWall, isSideWall);
        }
        bool CheckClimbEnable()
        {
            // only for ClimbUp At Up 
            //float fixYPos = -0.5f;
            //if (velocity.y > 0 && !_world.CheckClimbable(transform.position + new Vector3(directionVector.x, directionVector.y + fixYPos, 0)))

            // for Wherable
            bool canClimb = false;
            float fixYPos = -0.8f;
            
            if (_world.CheckNull(transform.position + new Vector3(directionVector.x, 1+ fixYPos, 0)))
                if (!_world.CheckNull(transform.position + new Vector3(directionVector.x, directionVector.y+ fixYPos, 0)))
                    if (_world.CheckNull(transform.position + new Vector3(directionVector.x, directionVector.y + 1+ fixYPos, 0)))
                        if (_world.CheckNull(transform.position + new Vector3(directionVector.x, directionVector.y + 2+ fixYPos, 0)))
                            canClimb = true;

            //if(canClimb)
            //    Debug.Log("can Climb");
            //else
            //    Debug.Log("no, Can't");
            return canClimb;

        }
    }
}
