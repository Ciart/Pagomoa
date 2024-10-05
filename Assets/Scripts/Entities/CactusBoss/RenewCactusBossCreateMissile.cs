using Ciart.Pagomoa.Entities.CactusBoss;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossCreateMissile : StateMachineBehaviour
    {
        private RenewCactusBoss _renewCactusBoss;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _renewCactusBoss = animator.GetComponent<RenewCactusBoss>();
            CreateMissile();
            animator.SetTrigger("CreateMissile");
        }
        private void CreateMissile()
        {
            if (_renewCactusBoss.CheckPlayerInRange())
            {
                Instantiate(_renewCactusBoss.missile, _renewCactusBoss.missileSpawnPoint.position, 
                    _renewCactusBoss.missileSpawnPoint.rotation);
            }
        }
    }
}
