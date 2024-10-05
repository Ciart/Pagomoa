using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossSurgeUp : StateMachineBehaviour
    {
        private RenewCactusBoss _renewCactusBoss;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _renewCactusBoss = animator.GetComponent<RenewCactusBoss>();
            _renewCactusBoss.rigid.velocity = Vector3.zero;
            UpSurging();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_renewCactusBoss.surgePoint)
                animator.SetBool("Surge", true);
        }

        private void UpSurging()
        {
            _renewCactusBoss.rigid.AddForce(Vector2.up * _renewCactusBoss.surgingSpeed);
        }
    }
}
