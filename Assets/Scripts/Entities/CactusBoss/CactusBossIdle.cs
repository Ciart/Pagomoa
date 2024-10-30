using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossIdle : StateMachineBehaviour
    {
        CactusBoss _cactusBoss;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cactusBoss = animator.GetComponent<CactusBoss>();
            _cactusBoss.ApplyAttackRate();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var player = GameManager.instance.player;

            if (_cactusBoss.CheckPlayerInRange())
            {
                if (!_cactusBoss.CheckAttackAble())
                {
                    return;
                }
                
                if (Random.Range(1, 3) == 1)
                {
                    animator.SetTrigger("UnderArm");
                }
                else
                {
                    animator.SetTrigger("TakeDown");
                }
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
