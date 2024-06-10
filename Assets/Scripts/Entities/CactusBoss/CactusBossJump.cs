using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class CactusBossSmallJump : StateMachineBehaviour
    {
        public float jumpForce = 20f;

        private CactusBoss _cactusBoss;

        private Vector2 _targetPosition;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cactusBoss = animator.GetComponent<CactusBoss>();
            _targetPosition = EntityManager.instance.player.transform.position;

            _cactusBoss.rigidbody.AddForce(new Vector2(0, jumpForce));
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var velocity = new Vector2((_targetPosition.x - animator.transform.position.x) * 80f * Time.deltaTime, _cactusBoss.rigidbody.velocity.y);

            _cactusBoss.rigidbody.velocity = velocity;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}
