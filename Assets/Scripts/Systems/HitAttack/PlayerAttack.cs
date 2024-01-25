using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.HitAttack
{
    [RequireComponent(typeof(Attack))]
    public class PlayerAttack : MonoBehaviour
    {
        private bool attack = false;
        private int attackDirection = 0;

        private PlayerController _playerController;

        private bool attackable = true;
        private float attackCooltime = 0.42f;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            GetComponent<PlayerInput>().Actions.Attack.started += context => { attack = true; };
        }
        private void FixedUpdate()
        {
            AttackDirectionDetect();
            if (attack && attackable)
            {
                NormatAttack();
                attack = false;
            }
        }
        void AttackDirectionDetect()
        {
            switch (_playerController.GetDirection()) {
                case Direction.Right:
                    attackDirection = 1;
                    break;
                case Direction.Left:
                    attackDirection = -1;
                    break;
            }
        }
        void NormatAttack()
        {
            attackable = false;
            Invoke("CanAttack", attackCooltime);

            _playerController.GetComponent<Animator>().SetFloat("attackDirection", attackDirection);
            _playerController.GetComponent<Animator>().SetTrigger("attack");
            //Debug.Log("기본공격!" + gameObject.name);

            Vector3 pointA, pointB, playerPosition = _playerController.transform.position;

            pointA = playerPosition + new Vector3(0 * attackDirection, 0.5f);
            pointB = playerPosition + new Vector3(1.5f * attackDirection, -0.5f);

            GetComponent<Attack>()._Attack(gameObject, GetComponent<PlayerStatus>().attackpower, pointA, pointB, 1);

        }
        void CanAttack()
        {
            attackable = true;
        }
    }
}
