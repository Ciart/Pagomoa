using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossHammerUp : StateMachineBehaviour
    {
        private RenewCactusBoss _renewCactusBoss;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("HammerUp상태 돌입");
            _renewCactusBoss = animator.GetComponent<RenewCactusBoss>();
            HammerUp();
            animator.SetTrigger("HammerDown");
        }
        private void HammerUp()
        {
            if (_renewCactusBoss.dir > 0)
            {
                _renewCactusBoss.arms[0].transform.rotation = Quaternion.Euler(0, 0, 60);
                _renewCactusBoss.upHammers.Add(_renewCactusBoss.arms[0]);
            }
            else if (_renewCactusBoss.dir < 0)
            {
                _renewCactusBoss.arms[1].transform.rotation = Quaternion.Euler(0, 0, -60); 
                _renewCactusBoss.upHammers.Add(_renewCactusBoss.arms[1]);
            }
        }
    }
}
