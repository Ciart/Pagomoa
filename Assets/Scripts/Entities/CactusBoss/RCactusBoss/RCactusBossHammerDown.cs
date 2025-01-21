using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossHammerDown : StateMachineBehaviour
    {
        private RCactusBoss _boss;
        [SerializeField] private float downSpeed;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
            _boss.hammers[0].GetComponent<RCactusBossHammer>().HammerCoroutine();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_boss.hammers[0].GetComponent<RCactusBossHammer>().doneInstantiate)
            {
                animator.SetTrigger("Idle");
                animator.ResetTrigger("HammerDown");
            }
        }
    }
}