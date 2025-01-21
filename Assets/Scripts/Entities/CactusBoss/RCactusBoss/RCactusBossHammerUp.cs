using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossHammerUp : StateMachineBehaviour
    {
        [SerializeField] private float upSpeed;
        [SerializeField] private float chaseSpeed;
        private float _elapsedTime;
        private RCactusBoss _boss;
        
        
        public float leftHammerMaxPosition;
        public float rightHammerMaxPosition;
        public float stateDuration;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _boss = animator.GetComponent<RCactusBoss>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HammerUp();
            
            if (_boss.hammers[0].transform.position.y >= leftHammerMaxPosition &&
                _boss.hammers[1].transform.position.y >= rightHammerMaxPosition)
            {
                _elapsedTime += Time.deltaTime;

                if (_elapsedTime >= stateDuration)
                {
                    animator.SetTrigger("HammerDown");
                    animator.ResetTrigger("HammerUp");
                    _elapsedTime = 0;
                }
            }
        }
        private void HammerUp()
        {
            _boss.hammers[0].transform.Translate(Vector2.up * upSpeed * Time.deltaTime);
            _boss.hammers[1].transform.Translate(Vector2.up * upSpeed * Time.deltaTime);
            
            Vector2 leftCurrentPos = _boss.hammers[0].transform.position;
            Vector2 rightCurrentPos = _boss.hammers[1].transform.position;
            
            leftCurrentPos.y = Mathf.Clamp(leftCurrentPos.y, 0, leftHammerMaxPosition);
            rightCurrentPos.y = Mathf.Clamp(rightCurrentPos.y, 0, rightHammerMaxPosition);
            
            _boss.hammers[0].transform.position = leftCurrentPos;
            _boss.hammers[1].transform.position = rightCurrentPos;

            if (leftCurrentPos.y >= leftHammerMaxPosition && rightCurrentPos.y >= rightHammerMaxPosition)
                ChasePlayer();                
        }

        private void ChasePlayer()
        {
            Vector2 leftCurrentPos = _boss.hammers[0].transform.position;
            Vector2 rightCurrentPos = _boss.hammers[1].transform.position;
            
            float playerPosX = GameManager.instance.player.transform.position.x;
            
            float chaseLeftPosition = Mathf.Lerp(leftCurrentPos.x, playerPosX, chaseSpeed * Time.deltaTime);
            float chaseRightPosition = Mathf.Lerp(rightCurrentPos.x, playerPosX, chaseSpeed * Time.deltaTime);
            
            _boss.hammers[0].transform.position = new Vector2(chaseLeftPosition, leftCurrentPos.y);
            _boss.hammers[1].transform.position = new Vector2(chaseRightPosition, rightCurrentPos.y);
        }
    }
}
