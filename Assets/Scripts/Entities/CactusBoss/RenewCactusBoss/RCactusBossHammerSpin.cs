using System;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class CactusBossHammerSpin : StateMachineBehaviour
    {
        private RCactusBoss _boss;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
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
            
            // _boss.hammers[0].GetComponent<Rigidbody2D>().MoveRotation(leftTargetRotation);
            // _boss.hammers[1].GetComponent<Rigidbody2D>().MoveRotation(rightTargetRotation);
            _boss.hammers[0].transform.rotation = Quaternion.RotateTowards(_boss.hammers[0].transform.rotation, leftTargetRotation, 45f * Time.deltaTime);
            _boss.hammers[1].transform.rotation = Quaternion.RotateTowards(_boss.hammers[1].transform.rotation, rightTargetRotation, 45f * Time.deltaTime);
        }
    }
}
