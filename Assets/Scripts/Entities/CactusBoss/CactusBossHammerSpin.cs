using System;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossHammerSpin : StateMachineBehaviour
    {
        private RenewCactusBoss _boss;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RenewCactusBoss>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HammerSpin();
            if (_boss.hammers[0].transform.rotation.eulerAngles.z >= 89.9f)
            {
                animator.SetTrigger("HammerCrash");
            }
        }

        private void HammerSpin()
        {
            Quaternion leftTargetRotation = Quaternion.Euler(0, 0, 90);
            Quaternion rightTargetRotation = Quaternion.Euler(0, 0, -90);
            _boss.hammers[0].transform.rotation = Quaternion.Lerp(_boss.hammers[0].transform.rotation, leftTargetRotation, Time.deltaTime * 5f);
            _boss.hammers[1].transform.rotation = Quaternion.Lerp(_boss.hammers[1].transform.rotation, rightTargetRotation, Time.deltaTime * 5f);
        }
    }
}
