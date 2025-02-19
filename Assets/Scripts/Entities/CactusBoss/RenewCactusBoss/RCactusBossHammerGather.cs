using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class CactusBossHammerGather : StateMachineBehaviour
    {
        private RCactusBoss _boss;

        public float gatherSpeed;
        
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
            GatherHammers();    
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_boss.hammers[0].GetComponent<RCactusBossHammer>().isGather)
            {
                animator.ResetTrigger("HammerGather");
                animator.SetTrigger("HammerChase");
            }
        }

        private void GatherHammers()
        {
            _boss.hammers[0].GetComponent<Rigidbody2D>().AddForce(Vector2.right * gatherSpeed, ForceMode2D.Impulse);
            _boss.hammers[1].GetComponent<Rigidbody2D>().AddForce(Vector2.left * gatherSpeed, ForceMode2D.Impulse);
        }
    }
}
