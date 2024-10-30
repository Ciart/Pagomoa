using System;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossSmallJumpEnd : StateMachineBehaviour
    {
        public string endTrigger;

        public int xSize = 3;

        public int ySize = 2;

        private CactusBoss _cactusBoss;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cactusBoss = animator.GetComponent<CactusBoss>();

            _cactusBoss.rigid.velocity = new Vector2(0, -20);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_cactusBoss.controller.isGrounded) {
                animator.SetTrigger(endTrigger);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var worldManager = WorldManager.instance;
            
            var position = WorldManager.ComputeCoords(_cactusBoss.transform.position);
            
            for (var i = -xSize; i <= xSize; i++) {
                for (var j = -ySize; j <= ySize; j++) {
                    worldManager.BreakGround(position.x + i,position.y + j, 5, true);
                }
            }
        }
    }
}
