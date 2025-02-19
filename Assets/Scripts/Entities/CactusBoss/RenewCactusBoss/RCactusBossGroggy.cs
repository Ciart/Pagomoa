using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class RCactusBossGroggy : StateMachineBehaviour
    {
        private RCactusBoss _boss;
        private float _timer;

        public float groggyTime;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
            _timer = 0f;
            _boss.patternCount = 0;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _timer += Time.deltaTime;

            if (_timer >= groggyTime)
            {
                animator.ResetTrigger("Groggy");                
                animator.SetTrigger("Idle");
            }
        }
    }
}
