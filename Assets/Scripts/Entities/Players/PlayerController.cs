using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Entities.Players
{
    [RequireComponent(typeof(PlayerStatus))]
    public partial class PlayerController : MonoBehaviour
    {
        public PlayerState state = PlayerState.Idle;

        public bool isGrounded = false;
        
        public PlayerStatus status;

        public PlayerStatus initialStatus;

        public float groundDistance = 0.125f;
        
        public float sideWallDistance = 1.0625f;
        
        [FormerlySerializedAs("inventoryDB")] public Inventory inventory;
        
        public DrillController drill;

        private Rigidbody2D _rigidbody;
        
        private PlayerInput _input;

        private PlayerMovement _movement;

        
        private Camera _camera;

        private WorldManager _world;

        private Direction _direction;

        private void Awake()
        {
            status = GetComponent<PlayerStatus>();
            initialStatus = status.copy();

            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();
            _digger = GetComponentInChildren<DrillController>();
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
            UpdateSound();

            _movement.isClimb = state == PlayerState.Climb;
            _movement.directionVector = _input.Move;

            _direction = DirectionUtility.ToDirection(_camera.ScreenToWorldPoint(_input.Look) - transform.position);

            if (_input.IsDig && state != PlayerState.Climb)
            {
                drill.isDig = true;
                drill.direction = _direction;
            }
            else if (_input.DigDirection.magnitude > 0.001f)
            {
                drill.isDig = true;
                drill.direction = DirectionUtility.ToDirection(_input.DigDirection);
            }
            else
            {
                drill.isDig = false;
            }

            TryJump();
            
            EventManager.Notify(new PlayerMove(transform.position));
        }

        private void UpdateIsGrounded()
        {
            var position = transform.position;
            position.y -= 1f; // TODO: 플레이어 영점 변경하면서 수정해야 함.
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
            if (status.hungry - value < 0) return true;
            status.hungry -= value;
            status.hungryAlter.Invoke(status.hungry, status.maxHungry);
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
