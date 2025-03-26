using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class CactusBossHammerDown : StateMachineBehaviour
    {
        private RCactusBoss _boss;
        [SerializeField] private float downSpeed;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
            _boss.hammers[0].GetComponent<RCactusBossHammer>().HammerCoroutine(downSpeed);
            _boss.patternCount++;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // if (_boss.patternCount == 3)
            // {
            //     animator.ResetTrigger("HammerDown");
            //     animator.SetTrigger("Groggy");
            // }
            if (_boss.hammers[0].GetComponent<RCactusBossHammer>().DoneInstantiate)
            {
                animator.ResetTrigger("HammerDown");
                animator.SetTrigger("Idle");
            }
        }
    }
}