using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Players
{
    [RequireComponent(typeof(PlayerStatus))]
    public partial class PlayerController : MonoBehaviour
    {
        public PlayerState state = PlayerState.Idle;

        public bool isGrounded = false;

        public GameObject drill;

        public PlayerStatus _status;

        public PlayerStatus _initialStatus;

        public float groundDistance = 1.125f;
        
        public float sideWallDistance = 1.0625f;

        private Rigidbody2D _rigidbody;
        
        private PlayerInput _input;

        private PlayerMovement _movement;

        private PlayerDigger _digger;
        
        private Camera _camera;

        private WorldManager _world;

        private Direction _direction;

        private void Awake()
        {
            _status = GetComponent<PlayerStatus>(); // ���� �ʱ�ȭ
            _initialStatus = _status.copy();  // �⺻ ���� ����

            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();
            _digger = GetComponent<PlayerDigger>();

            _camera = Camera.main;
            _world = WorldManager.instance;
        }

        private void TryJump()
        {
            if (!_input.IsJump || !isGrounded || state is PlayerState.Climb or PlayerState.Fall or PlayerState.Jump)
            {
                return;
            }

            state = PlayerState.Jump;
            _movement.Jump();
        }

        private void Update()
        {
            UpdateState();

            _movement.isClimb = state == PlayerState.Climb;
            _movement.directionVector = _input.Move;

            _direction = DirectionUtility.ToDirection(_camera.ScreenToWorldPoint(_input.Look) - transform.position);

            if (_input.IsDig && state != PlayerState.Climb)
            {
                _digger.isDig = true;
                _digger.direction = _direction;
                drill.SetActive(true);
            }
            else if (_input.DigDirection.magnitude > 0.001f)
            {
                _digger.isDig = true;
                _digger.direction = DirectionUtility.ToDirection(_input.DigDirection);
                drill.SetActive(true);
            }
            else
            {
                _digger.isDig = false;
                drill.SetActive(false);
            }

            TryJump();
        }

        private void UpdateIsGrounded()
        {
            var position = transform.position;
            var hit = Physics2D.Raycast(position, Vector2.down, groundDistance, LayerMask.GetMask("Platform"));
            Debug.DrawRay(position, Vector2.down * groundDistance, Color.green);

            isGrounded = (bool)hit.collider;
        }

        private void UpdateIsSideWall()
        {
            var directionVector = _input.Move.x switch
            {
                <= -0.0001f => Vector2.left,
                >= 0.0001f => Vector2.right,
                _ => Vector2.zero
            };

            if (directionVector == Vector2.zero)
            {
                _movement.isSideWall = false;
                return;
            }

            var position = transform.position;

            var hit = Physics2D.Raycast(position, directionVector, sideWallDistance,
                LayerMask.GetMask("Platform"));
            Debug.DrawRay(position, directionVector * sideWallDistance, Color.green);

            if (!hit.collider || hit.collider.transform.position.y > 0f)
            {
                _movement.isSideWall = false;
                return;
            }

            _movement.isSideWall = true;
        }
        
        public bool Hungry(float value)
        {
            if (_status.hungry - value < 0) return true;
            _status.hungry -= value;
            _status.hungryAlter.Invoke(_status.hungry, _status.maxHungry);
            return false;
        }
        
        private void FixedUpdate()
        {
            UpdateIsGrounded();
            UpdateIsSideWall();
        }

        public Direction GetDirection()
        {
            return _direction;
        }
    }
}