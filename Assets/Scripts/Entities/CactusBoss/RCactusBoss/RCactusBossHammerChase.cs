using Ciart.Pagomoa.Entities.CactusBoss;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.cactusBoss
{
    public class CactusBossHammerChase : StateMachineBehaviour
    {
        public float chaseSpeed;
        public float stateDuration;
        
        private RCactusBoss _boss;
        private float _elapsedTime;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ChasePlayer();

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= stateDuration)
            {
                animator.SetTrigger("HammerSmashDown");
                animator.ResetTrigger("HammerChase");
                _elapsedTime = 0;
            }
        }

        private void ChasePlayer()
        {
            Vector2 leftCurrentPos = _boss.hammers[0].transform.position;
            Vector2 rightCurrentPos = _boss.hammers[1].transform.position;
            
            float playerPosX = GameManager.instance.player.transform.position.x;
            
            float chaseLeftPosition = Mathf.Lerp(leftCurrentPos.x, playerPosX - 1.7f, chaseSpeed * Time.deltaTime);
            float chaseRightPosition = Mathf.Lerp(rightCurrentPos.x, playerPosX + 1.7f, chaseSpeed * Time.deltaTime);
            
            _boss.hammers[0].transform.position = new Vector2(chaseLeftPosition, leftCurrentPos.y);
            _boss.hammers[1].transform.position = new Vector2(chaseRightPosition, rightCurrentPos.y);
        }
    }
}
