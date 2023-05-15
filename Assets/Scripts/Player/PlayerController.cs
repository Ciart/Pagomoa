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

        public Status status;

        private Rigidbody2D _rigidbody;

        private Animator _animator;

        private PlayerInput _input;

        private PlayerMovement _movement;
        
        private MapManager _map;
        
        public event EventHandler<ChangeStateEventArgs> ChangeState;

        private void Awake()
        {
            status = GetComponent<Status>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();
            
            _map = MapManager.Instance;
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (isGrounded && state is PlayerState.Climb or PlayerState.Fall)
            {
                return;
            }
            
            state = PlayerState.Jump;
            _movement.Jump();
        }

        private void Update()
        {
            UpdateState();
            
            _actions.Player.Jump.performed += OnJump;
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