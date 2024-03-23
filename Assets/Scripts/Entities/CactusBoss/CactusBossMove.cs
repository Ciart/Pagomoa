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

            var player = EntityManager.instance.player;
            var velocity = new Vector2((player.transform.position.x - animator.transform.position.x) * _cactusBoss.speed * Time.deltaTime, _cactusBoss.rigidbody.velocity.y);

            _cactusBoss.rigidbody.velocity = velocity;
            _cactusBoss.spriteRenderer.flipX = velocity.x > 0;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}
