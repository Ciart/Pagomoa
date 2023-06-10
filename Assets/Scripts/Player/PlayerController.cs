using System;
using Constants;
using Maps;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Status))]
    public partial class PlayerController : MonoBehaviour
    {
        public PlayerState state = PlayerState.Idle;

        public bool isGrounded = false;

        public GameObject drill;

        public float groundDistance = 1.125f;
        
        public float sideWallDistance = 1.0625f;

        private Rigidbody2D _rigidbody;

        private PlayerInput _input;

        private PlayerMovement _movement;

        private PlayerDigger _digger;

        private MapManager _map;

        private Direction _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();
            _digger = GetComponent<PlayerDigger>();

            _map = MapManager.Instance;
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

            _direction = DirectionUtility.ToDirection(_input.Move);

            if (_input.IsDig && state != PlayerState.Climb)
            {
                _digger.isDig = true;
                _digger.direction = _direction == Direction.Up ? Direction.Down : _direction;
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

            if (!hit.collider)
            {
                _movement.isSideWall = false;
                return;
            }

            _movement.isSideWall = true;
        }

        private void FixedUpdate()
        {
            UpdateIsGrounded();
            UpdateIsSideWall();
        }
    }
}