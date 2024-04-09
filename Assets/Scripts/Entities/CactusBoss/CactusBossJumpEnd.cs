using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossSmallJumpEnd : StateMachineBehaviour
    {
        private CactusBoss _cactusBoss;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cactusBoss = animator.GetComponent<CactusBoss>();

            _cactusBoss.rigidbody.velocity = new Vector2(0, -20);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_cactusBoss.controller.isGrounded) {
                _cactusBoss.ResetArm();
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var position = WorldManager.ComputeCoords(_cactusBoss.transform.position);
            
            for (var i = -3; i <= 3; i++) {
                WorldManager.instance.BreakGround(position.x + i,position.y - 1, 5);
            }
        }
    }
}
