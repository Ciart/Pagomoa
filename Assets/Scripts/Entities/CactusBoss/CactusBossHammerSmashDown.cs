using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossHammerSmashDown : StateMachineBehaviour
    {
        private RenewCactusBoss _boss;
        private float _elapsedTime = 0;
        private float _stateDuration = 5;
        
        public float downSpeed;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RenewCactusBoss>();
            
            _boss.hammers[0].GetComponent<Rigidbody2D>().AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);
            _boss.hammers[1].GetComponent<Rigidbody2D>().AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _stateDuration)
            {
                animator.SetTrigger("Idle");
                animator.ResetTrigger("HammerSmashDown");
            }
        }
    }
}
