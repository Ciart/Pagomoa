using System;

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
    
    public partial class PlayerController
    {
        private bool CheckClimb()
        {
            return _input.IsClimb && _map.CheckClimbable(transform.position);
        }

        private bool CheckFall()
        {
            return !isGrounded && _rigidbody.velocity.y < 0;
        }

        private bool CheckJump()
        {
            return state == PlayerState.Jump;
        }

        private bool CheckDig()
        {
            return _input.IsDig;
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
            else if (CheckDig())
            {
                return PlayerState.Dig;
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
                ChangeState?.Invoke(this, new ChangeStateEventArgs(state));
            }
        }
    }
}