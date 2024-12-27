using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossMove : StateMachineBehaviour
    {
        private CactusBoss _cactusBoss;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cactusBoss = animator.GetComponent<CactusBoss>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_cactusBoss.CheckPlayerInRange())
            {
                animator.SetTrigger("Idle");
                return;
            }

            var player = GameManager.instance.player;
            var velocity = new Vector2((player.transform.position.x - animator.transform.position.x) * _cactusBoss.speed * Time.deltaTime, _cactusBoss.rigid.linearVelocity.y);

            _cactusBoss.rigid.linearVelocity = velocity;
            _cactusBoss.spriteRenderer.flipX = velocity.x > 0;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cactusBoss.rigid.linearVelocity = Vector2.zero;
        }
    }
}
