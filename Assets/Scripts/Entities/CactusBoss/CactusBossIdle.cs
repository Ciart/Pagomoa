using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossIdle : StateMachineBehaviour
    {
        CactusBoss _cactusBoss;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cactusBoss = animator.GetComponent<CactusBoss>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var player = EntityManager.instance.player;

            if (_cactusBoss.CheckPlayerInRange())
            {
                if (!_cactusBoss.CheckAttackAble())
                {
                    return;
                }
                
                animator.SetTrigger("Attack");
            }
            else
            {
                animator.SetTrigger("Move");
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
    }
}
