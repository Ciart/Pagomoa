using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class CactusBossHammerSmashDown : StateMachineBehaviour
    {
        private RCactusBoss _boss;
        private float _elapsedTime;
        private float _stateDuration = 5;
        
        public float downSpeed;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
            _boss.hammers[0].GetComponent<Rigidbody2D>().AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);
            _boss.hammers[1].GetComponent<Rigidbody2D>().AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);
            _elapsedTime = 0f;
            _boss.patternCount++;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime >= _stateDuration)
            {
                if (_boss.patternCount == 3)
                {
                    animator.ResetTrigger("HammerSmashDown");
                    animator.SetTrigger("Groggy");
                }
                else
                {
                    animator.ResetTrigger("HammerSmashDown");
                    animator.SetTrigger("Idle");
                }
            }
        }
    }
}
