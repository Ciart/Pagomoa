using Ciart.Pagomoa.Entities.CactusBoss;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossIdle : StateMachineBehaviour
    {
        private RenewCactusBoss _renewCactusBoss;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("IDLE 상태 돌입");
            _renewCactusBoss = animator.GetComponent<RenewCactusBoss>();
            _renewCactusBoss.ApplyAttackRate();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_renewCactusBoss.CheckPlayerInRange())
            {
                // if (!_renewCactusBoss.CheckAttackAble())
                //     return;
                if(_renewCactusBoss.arms[0].activeSelf == false && _renewCactusBoss.arms[1].activeSelf == false)
                    animator.SetBool("Surge", true);
                else
                    animator.SetTrigger("HammerUp");

                // animator.SetTrigger("CreateMissile");
                
            }
        }
    }
}
