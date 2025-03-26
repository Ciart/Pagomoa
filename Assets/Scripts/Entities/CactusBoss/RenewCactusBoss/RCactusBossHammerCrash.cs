using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class CactusBossHammerCrash : StateMachineBehaviour
    {
        private RCactusBoss _boss;
        public float speed;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
            animator.ResetTrigger("HammerSpin");
            HammerCrash();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_boss.hammers[0].GetComponent<RCactusBossHammer>().DoneInstantiate)
            {
                animator.SetTrigger("ResetRotation");
                animator.ResetTrigger("HammerCrash");
            }
        }

        private void HammerCrash()
        {
            _boss.hammers[0].GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed, ForceMode2D.Impulse);
            _boss.hammers[1].GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed, ForceMode2D.Impulse);
        }
    }
}
