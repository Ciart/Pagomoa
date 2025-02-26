using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Worlds;
using System;
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

        public float climbSpeed = 250f;

        public float jumpForce = 650f;
        
        public float fallForce = 200f;

        // TODO: 최대 거리 검사가 필요 함.
        public Vector2 directionVector = Vector2.zero;

        public bool isStepUp => _animator.GetBool(AnimatorEndClimb);

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
        
        private EntityController _entityController;

        private WorldManager _world;

        private bool _isJump;
        
        private bool _isFall;

        private Vector2 _playerMoveMinArea = Vector2.zero;
        private Vector2 _playerMoveMaxArea = Vector2.zero;

        private Vector3 _moveDelta;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _entityController = GetComponent<EntityController>();

            _world = WorldManager.instance;

            Init();
        }

        private void OnEnable()
        {
            EventManager.AddListener<LevelChangedEvent>(OnLevelChanged);
            EventManager.AddListener<WorldCreatedEvent>(OnWorldCreated);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<LevelChangedEvent>(OnLevelChanged);
            EventManager.RemoveListener<WorldCreatedEvent>(OnWorldCreated);
        }

        private void Init()
        {
            var currentLevel = _world.worldGenerator;

            SetMoveArea(currentLevel.left, currentLevel.right, currentLevel.bottom, currentLevel.top);
        }

        private void OnWorldCreated(WorldCreatedEvent e)
        {
            var currentLevel = e.world.currentLevel;

            SetMoveArea(currentLevel.left, currentLevel.right, currentLevel.bottom, currentLevel.top);
        }

        private void OnLevelChanged(LevelChangedEvent e)
        {
            var currentLevel = e.level;

            SetMoveArea(currentLevel.left, currentLevel.right, currentLevel.bottom, currentLevel.top);
        }

        private void SetMoveArea(int left, int right, int bottom, int top)
        {
            float sideFlex = 0.3f;

            _playerMoveMinArea.x = -left + sideFlex;
            _playerMoveMaxArea.x = right - sideFlex;

            _playerMoveMinArea.y = -bottom;
            _playerMoveMaxArea.y = top;
        }

        public void Jump()
        {
            _isJump = true;
        }

        public void Fall()
        {
            _isFall = true;
        }
        
        private void UpdateClimb()
        {
            if (_animator.GetBool(AnimatorEndClimb)) return;

            var velocity = directionVector * (climbSpeed * Time.deltaTime);
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("endClimb"))
            {
                _animator.SetBool(AnimatorEndClimb, false);
            }

            
            if(velocity.y > 0 && CheckClimbEnable())
            {
                velocity.y = 0;

                if (isSideWall && !_animator.GetCurrentAnimatorStateInfo(0).IsName("endClimb"))
                {
                    _animator.SetBool(AnimatorEndClimb, true);
                }
            }
            _rigidbody.linearVelocity = velocity;
            _rigidbody.gravityScale = 0;
        }

        private void EndClimbLeft()
        {
            Vector3 movePos = new Vector3(-0.4f, 1.04f);
            if (isClimb)
                transform.position += movePos;
            _animator.SetBool(AnimatorEndClimb, false);
        }

        private void EndClimbRight()
        {
            Vector3 movePos = new Vector3(0.4f, 1.04f);
            if (isClimb)
                transform.position += movePos;
            _animator.SetBool(AnimatorEndClimb, false);
        }

        private void UpdateWalk()
        {
            if (_animator.GetBool(AnimatorEndClimb)) return;

            var velocity = new Vector2(directionVector.x * _entityController.Speed * Time.deltaTime, _rigidbody.linearVelocity.y);

            _rigidbody.linearVelocity = velocity;
            _rigidbody.gravityScale = gravityScale;
        }

        private void CheckDomain()
        {
            _moveDelta.x = Mathf.Clamp(transform.position.x, _playerMoveMinArea.x, _playerMoveMaxArea.x);
            _moveDelta.y = Mathf.Clamp(transform.position.y, _playerMoveMinArea.y, _playerMoveMaxArea.y);

            transform.position = _moveDelta;
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

            if (_isFall)
            {
                _rigidbody.AddForce(new Vector2(0, -fallForce));
                _isFall = false;
            }

            if (isClimb)
            {
                UpdateClimb();
            }
            else
            {
                UpdateWalk();
            }

            CheckDomain();

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
        private bool CheckClimbEnable()
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
