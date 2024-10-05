using Ciart.Pagomoa.Constants;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Players
{
    public class PlayerAttack : MonoBehaviour
    {
        bool attack = false;
        int attackDirection = 0;

        PlayerController _playerController;

        bool attackable = true;
        float attackCooltime = 0.42f;

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
            switch (_playerController.GetDirection())
            {
                case Direction.Right:
                    attackDirection = 1;
                    break;
                case Direction.Left:
                    attackDirection = -1;
                    break;
            }
        }

        // void Attack(GameObject attacker, float damage, Vector3 startPoint, Vector3 endPoint, int canHitCount = 1)
        // {
        //     int count = canHitCount;
        //     Collider2D[] deffenders = Physics2D.OverlapAreaAll(startPoint, endPoint);
        //     foreach (Collider2D deffender in deffenders)
        //     {
        //         EntityController target = deffender.GetComponent<EntityController>();

        //         if (!target) continue;

        //         target.TakeDamage(damage, attacker: attacker, flag: DamageFlag.Melee);
        //     }
        // }

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

            // Attack(gameObject, GetComponent<PlayerStatus>().attackpower, pointA, pointB, 1);
        }

        void CanAttack()
        {
            attackable = true;
        }
    }
}
