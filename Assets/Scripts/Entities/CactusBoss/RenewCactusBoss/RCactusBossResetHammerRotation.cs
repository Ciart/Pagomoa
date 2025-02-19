using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class CactusBossResetHammerRotation : StateMachineBehaviour
    {
        private RCactusBoss _boss;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
            _boss.patternCount++;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HammerSpin();
            if (_boss.hammers[0].transform.rotation.eulerAngles.z <= 0.1)
            {
                if (_boss.patternCount == 3)
                {
                    animator.ResetTrigger("ResetRotation");
                    animator.SetTrigger("Groggy");
                }
                else
                {
                    animator.ResetTrigger("ResetRotation");
                    animator.SetTrigger("Idle");
                } 
            }
        }
        
        private void HammerSpin()
        {
            Quaternion leftTargetRotation = Quaternion.Euler(0, 0, 0);
            Quaternion rightTargetRotation = Quaternion.Euler(0, 0, 0);
            _boss.hammers[0].transform.rotation = Quaternion.Lerp(_boss.hammers[0].transform.rotation, leftTargetRotation, Time.deltaTime * 5f);
            _boss.hammers[1].transform.rotation = Quaternion.Lerp(_boss.hammers[1].transform.rotation, rightTargetRotation, Time.deltaTime * 5f);
        }
    }
}
