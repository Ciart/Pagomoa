using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBossIdle : StateMachineBehaviour
    {
       
        private RenewCactusBoss _renewCactusBoss;
        private bool _isReposed;
        private int _randomNumber;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _renewCactusBoss = animator.GetComponent<RenewCactusBoss>();
            _randomNumber = Random.Range(0, 3);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_renewCactusBoss.hammers[0].GetComponent<CactusBossHammer>().doneInstantiate)
            {
                ResetHammersPos();
                if (AreVectorsEqual(_renewCactusBoss.hammers[0].transform.position, _renewCactusBoss.initialPos[0].position, 0.1f))
                {
                    _renewCactusBoss.hammers[0].GetComponent<CactusBossHammer>().doneInstantiate = false;
                    _isReposed = true;
                }
            }

            if (_renewCactusBoss.CheckPlayerInRange() && _isReposed)
            {
                switch (_randomNumber)
                {
                    case 0:
                        animator.SetTrigger("HammerUp");
                        break;
                    case 1:
                        animator.SetTrigger("HammerSpin");
                        break;
                    case 2:
                        animator.SetTrigger("HammerSmashUp");
                        break;
                }
                _isReposed = false;
                animator.ResetTrigger("Idle");
            }
        }
        
        private void ResetHammersPos()
        {
            _renewCactusBoss.hammers[0].transform.position = 
                Vector3.Lerp(_renewCactusBoss.hammers[0].transform.position, _renewCactusBoss.initialPos[0].position, 3.0f * Time.deltaTime);
            _renewCactusBoss.hammers[1].transform.position = 
                Vector3.Lerp(_renewCactusBoss.hammers[1].transform.position, _renewCactusBoss.initialPos[1].position, 3.0f * Time.deltaTime);
        }
        private bool AreVectorsEqual(Vector3 a, Vector3 b, float epsilon = 0.1f) {
            return Vector3.Distance(a, b) < epsilon;
        }
    }
}
