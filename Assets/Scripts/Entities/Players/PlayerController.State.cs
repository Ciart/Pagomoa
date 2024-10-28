using System;

namespace Ciart.Pagomoa.Entities.Players
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Climb,
    }

    public partial class PlayerController
    {
        private bool CheckClimb()
        {
            return _input.IsClimb && _world.CheckClimbable(transform.position);
        }

        private bool CheckFall()
        {
            return (!_input.IsJump || _rigidbody.velocity.y <= 0) && !isGrounded;
        }

        private bool CheckJump()
        {
            return state == PlayerState.Jump && _rigidbody.velocity.y > 0;
        }

        private bool CheckWalk()
        {
            return Math.Abs(_input.Move.x) > 0;
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
            else if (CheckWalk())
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
                OnChangedState(state);
            }
        }
    }
}