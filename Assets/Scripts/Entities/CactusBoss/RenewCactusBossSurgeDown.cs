using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossSurgeDown : StateMachineBehaviour
    {
        private RenewCactusBoss _renewCactusBoss;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _renewCactusBoss = animator.GetComponent<RenewCactusBoss>();
            _renewCactusBoss.rigid.velocity = Vector3.zero;
            DownSurging();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_renewCactusBoss.surgePoint)
                animator.SetBool("Surge", false);
        }
        private void DownSurging()
        {
                _renewCactusBoss.footHold.GetComponent<EdgeCollider2D>().enabled = false;
                _renewCactusBoss.rigid.AddForce(Vector2.down * _renewCactusBoss.surgingSpeed);
        }
    }
}
