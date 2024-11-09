using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossRightHammering : StateMachineBehaviour
    {
        private RenewCactusBoss _renewCactusBoss;
        public GameObject shockWave;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("RightHammerUp상태 돌입");
            _renewCactusBoss = animator.GetComponent<RenewCactusBoss>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetTrigger("Hammering");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject leftObj = Instantiate(shockWave, _renewCactusBoss.hammerPoints[2].position, Quaternion.identity);
            leftObj.GetComponent<RenewCactusBossShockWave>().dir = -1f;
            GameObject rightObj = Instantiate(shockWave, _renewCactusBoss.hammerPoints[3].position, Quaternion.identity);
            rightObj.GetComponent<RenewCactusBossShockWave>().dir = 1f;
            Debug.Log("Exit!");
        }
    }
}
