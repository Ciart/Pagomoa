using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Dig,
        Climb,
    }

    public class ChangeStateEventArgs : EventArgs
    {
        public PlayerState State;

        public ChangeStateEventArgs(PlayerState state)
        {
            State = state;
        }
    }

    [RequireComponent(typeof(Status))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerState state = PlayerState.Idle;

        public bool isGround = false;

        public float GroundHeight = 0;

        public float oxygen_consume = 0.001f;

        public Status status;

        private Rigidbody2D _rigidbody;

        private Animator _animator;

        private PlayerMovement _movement;

        private InputActions _actions;

        private Vector2 _moveInput;

        public Vector2 MoveInput
        {
            get => _moveInput;
        }

        public UnityEvent<float, float> oxygen_alter;
        public UnityEvent<float, float> hungry_alter;
        public UnityEvent<float> direction_alter;

        public event EventHandler<ChangeStateEventArgs> ChangeState;

        private void Awake()
        {
            status = GetComponent<Status>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _movement = GetComponent<PlayerMovement>();

            _actions = new InputActions();
            _actions.Player.Jump.performed += OnJump;
        }

        private void OnEnable()
        {
            _actions.Player.Enable();
        }

        private void OnDisable()
        {
            _actions.Player.Disable();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (state is not PlayerState.Climb or PlayerState.Fall)
            {
                state = PlayerState.Jump;
                _movement.Jump();
            }
        }

        private bool CheckClimb()
        {
            return _actions.Player.Climb.IsPressed() && MapManager.Instance.CheckClimbable(transform.position);
        }

        private bool CheckFall()
        {
            return !isGround && _rigidbody.velocity.y < 0;
        }

        private bool CheckJump()
        {
            return state == PlayerState.Jump;
        }

        private bool CheckDig()
        {
            return _actions.Player.Dig.IsPressed();
        }

        private bool CheckMove()
        {
            return Math.Abs(MoveInput.x) > 0;
        }

        private PlayerState CheckState()
        {
            if (CheckClimb())
            {
                return PlayerState.Climb;
            }
            else if (CheckFall())
            {
                return PlayerState.Fall;
            }
            else if (CheckJump())
            {
                return PlayerState.Jump;
            }
            else if (CheckDig())
            {
                return PlayerState.Dig;
            }
            else if (CheckMove())
            {
                return PlayerState.Walk;
            }

            return PlayerState.Idle;
        }

        private void UpdateState()
        {
            var prevState = state;

            state = CheckState();

            if (prevState != state)
            {
                ChangeState?.Invoke(this, new(state));
            }
        }

        private void UpdateOxygen()
        {
            if (transform.position.y < GroundHeight && status.oxygen >= status.min_oxygen)
            {
                status.oxygen -= Mathf.Abs(transform.position.y) * oxygen_consume * Time.deltaTime;

                if (status.oxygen < status.min_oxygen)
                {
                    status.oxygen = status.min_oxygen;
                }
            }
            else if (status.oxygen < status.max_oxygen)
            {
                status.oxygen += Mathf.Abs(transform.position.y) * oxygen_consume * Time.deltaTime;
            }

            oxygen_alter.Invoke(status.oxygen, status.max_oxygen);
        }

        private void Update()
        {
            _moveInput = _actions.Player.Move.ReadValue<Vector2>();

            UpdateState();
            UpdateOxygen();
        }

        private void FixedUpdate()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, 1f + 0.0625f, LayerMask.GetMask("Platform"));
            Debug.DrawRay(transform.position, Vector2.down * (1f + 0.0625f), Color.green);

            if (hit.collider)
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
        }

        public void SetDirection(float direction)
        {
            if (direction != 0)
            {
                WalkingAnimation(direction);
                Vector3 Direction = new Vector3(direction, 1, 1);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
                direction_alter.Invoke(direction);
            }
            else
            {
                _animator.SetBool("goLeft", false);
                _animator.SetBool("goRight", false);
            }
        }

        public void WalkingAnimation(float direction)
        {
            if (direction == 1) { _animator.SetBool("goRight", true); }
            else { _animator.SetBool("goLeft", true); }
        }

        public void ActiveClimbingAnimation()
        {
            if (_animator.GetInteger("climb") == 0)
            {
                _animator.SetInteger("climb", 1);
            }
        }

        public void DisableClimbingAnimation()
        {
            if (_animator.GetInteger("climb") == 1 || _animator.GetInteger("climb") == 2)
            {
                _animator.SetInteger("climb", 0);
            }
        }

        public void ActiveLeftTileClimbingAnimation()
        {
            if (_animator.GetInteger("tileDirection") == 0)
            {
                _animator.SetInteger("climb", 2);
                _animator.SetInteger("tileDirection", -1);
            }
        }

        public void ActiveRightTileClimbingAnimation()
        {
            if (_animator.GetInteger("tileDirection") == 0)
            {
                _animator.SetInteger("climb", 2);
                _animator.SetInteger("tileDirection", 1);
            }
        }

        public void DisableTileClimbingAnimation()
        {
            if (_animator.GetInteger("tileDirection") == 1 || _animator.GetInteger("tileDirection") == -1)
            {
                _animator.SetInteger("tileDirection", 0);
            }
        }

        public void ActiveLeftTileClimbingEndMotion()
        {
            _animator.SetBool("endClimb", true);
        }

        public void DisableLeftTileClimbingEndMotion()
        {
            _animator.SetBool("endClimb", false);
        }
    }
}