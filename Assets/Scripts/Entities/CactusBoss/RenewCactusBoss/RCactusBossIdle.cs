using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class RenewCactusBossIdle : StateMachineBehaviour
    {
       
        private RCactusBoss _renewCactusBoss;
        private bool _isReposed;
        private int _randomNumber;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _renewCactusBoss = animator.GetComponent<RCactusBoss>();
            // _renewCactusBoss.hammers[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            // _renewCactusBoss.hammers[1].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            if (_renewCactusBoss.patternCount >= 3)
            {
                _renewCactusBoss.state = RCactusBoss.State.Groggy;
            }
            else
            {
                _randomNumber = Random.Range(0, 3);
                _renewCactusBoss.state = (RCactusBoss.State)2;
            }
            Debug.Log("그로기 전, 후 보스의 state : " + _renewCactusBoss.state);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_renewCactusBoss.hammers[0].GetComponent<RCactusBossHammer>().DoneInstantiate)
            {
                ResetHammersPos();
                if (AreVectorsEqual(_renewCactusBoss.hammers[0].transform.position, _renewCactusBoss.initialPos[0].position))
                {
                    _renewCactusBoss.hammers[0].GetComponent<RCactusBossHammer>().DoneInstantiate = false;
                    _isReposed = true;
                }
            }
            
            if (_renewCactusBoss.CheckPlayerInRange() && _isReposed)
            {
                switch (_renewCactusBoss.state)
                {
                    case RCactusBoss.State.Hammer:
                        animator.SetTrigger("HammerUp");
                        break;
                    case RCactusBoss.State.Spin:
                        animator.SetTrigger("HammerSpin");
                        break;
                    case RCactusBoss.State.Smash:
                        animator.SetTrigger("HammerSmashUp");
                        break;
                    case RCactusBoss.State.Groggy:
                        animator.SetTrigger("Groggy");
                        break;
                }
                _isReposed = false;
                animator.ResetTrigger("Idle");
            }
        }
        
        private void ResetHammersPos()
        {
            for (int i = 0; i < _renewCactusBoss.hammers.Length; i++)
            {
                Transform hammerTransform = _renewCactusBoss.hammers[i].transform;
                Transform targetTransform = _renewCactusBoss.initialPos[i];
                
                hammerTransform.position = Vector3.Lerp(hammerTransform.position, targetTransform.position, 3.0f * Time.deltaTime);
            }
        }
        private bool AreVectorsEqual(Vector3 a, Vector3 b, float epsilon = 0.1f) {
            return Vector3.Distance(a, b) < epsilon;
        }
    }
}
