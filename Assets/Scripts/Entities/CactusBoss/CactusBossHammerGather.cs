using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossHammerGather : StateMachineBehaviour
    {
        private RenewCactusBoss _boss;

        public float gatherSpeed;
        
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RenewCactusBoss>();
            GatherHammers();    
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_boss.hammers[0].GetComponent<CactusBossHammer>().isGather)
            {
                animator.SetTrigger("HammerChase");
                animator.ResetTrigger("HammerGather");
            }
        }

        private void GatherHammers()
        {
            _boss.hammers[0].GetComponent<Rigidbody2D>().AddForce(Vector2.right * gatherSpeed, ForceMode2D.Impulse);
            _boss.hammers[1].GetComponent<Rigidbody2D>().AddForce(Vector2.left * gatherSpeed, ForceMode2D.Impulse);
        }
    }
}
