using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossHammerDown : StateMachineBehaviour
    {
        private RenewCactusBoss _boss;
        [SerializeField] private float downSpeed;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RenewCactusBoss>();
            _boss.hammers[0].GetComponent<CactusBossHammer>().HammerCoroutine();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_boss.hammers[0].GetComponent<CactusBossHammer>().doneInstantiate)
            {
                animator.SetTrigger("Idle");
                animator.ResetTrigger("HammerDown");
            }
        }
    }
}