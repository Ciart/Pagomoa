using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Status))]
    public partial class PlayerController : MonoBehaviour
    {
        public PlayerState state = PlayerState.Idle;

        public bool isGrounded = false;

        private Rigidbody2D _rigidbody;

        private PlayerInput _input;

        private PlayerMovement _movement;

        private PlayerDigger _digger;
        
        private MapManager _map;

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
            _movement.direction = _input.Move;
            
            TryJump();
        }
        
        private void UpdateIsGrounded()
        {
            var position = transform.position;
            var hit = Physics2D.Raycast(position, Vector2.down, 1f + 0.0625f, LayerMask.GetMask("Platform"));
            Debug.DrawRay(position, Vector2.down * (1f + 0.0625f), Color.green);

            isGrounded = (bool)hit.collider;
        }

        private void FixedUpdate()
        {
            UpdateIsGrounded();
        }
    }
}