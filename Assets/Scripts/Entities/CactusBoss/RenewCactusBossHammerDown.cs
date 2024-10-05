using Ciart.Pagomoa.Entities.CactusBoss;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossHammerDown : StateMachineBehaviour
    {
        private RenewCactusBoss _renewCactusBoss;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("HammerDown상태 돌입");
            _renewCactusBoss = animator.GetComponent<RenewCactusBoss>();
            HammerDown(_renewCactusBoss.upHammers[0]);
            animator.SetTrigger("HammerUp");
        }
        
        private void HammerDown(GameObject hammer)
        {
            hammer.transform.rotation = Quaternion.Euler(0, 0, 0);
            _renewCactusBoss.upHammers.Remove(_renewCactusBoss.upHammers[0]);
        }
    }
}
