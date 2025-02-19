using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class CactusBossHammerSmashUp : StateMachineBehaviour
    {
        private RCactusBoss _boss;
        
        [SerializeField] private float upSpeed;
        [SerializeField] private float chaseSpeed;
        private float _elapsedTime;

        
        public float hammerMaxPosition;
    
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HammerUp();

            if (_boss.hammers[0].transform.position.y >= hammerMaxPosition)
            {
                animator.ResetTrigger("HammerSmashUp");
                animator.SetTrigger("HammerGather");
            }
        }
        
        private void HammerUp()
        {
            _boss.hammers[0].transform.Translate(Vector2.up * upSpeed * Time.deltaTime);
            _boss.hammers[1].transform.Translate(Vector2.up * upSpeed * Time.deltaTime);
            
            Vector2 leftCurrentPos = _boss.hammers[0].transform.position;
            Vector2 rightCurrentPos = _boss.hammers[1].transform.position;
            
            leftCurrentPos.y = Mathf.Clamp(leftCurrentPos.y, 0, hammerMaxPosition);
            rightCurrentPos.y = Mathf.Clamp(rightCurrentPos.y, 0, hammerMaxPosition);
            
            _boss.hammers[0].transform.position = leftCurrentPos; 
            _boss.hammers[1].transform.position = rightCurrentPos;

            
        }
    }
}
