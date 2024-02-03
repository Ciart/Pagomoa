using System.Collections;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public class Monster : MonoBehaviour
    {
        protected Animator _animator;
        protected MonsterController _controller;

        public MonsterStatus status;
        public MonsterState state;

        public bool isGround = true;
        public Transform groundCheck;

        public GameObject target;

        // public Attack _attack;
        public int direction;
        public enum MonsterState
        {
            Active,           // ��� Ȱ��
            Sleep,            // ��
            Chase,            // �߰�
            Hit,              // �ǰ�
            Die               // ����
        }
        void GroundCheck()
        {
            isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, LayerMask.GetMask("Platform")) ? true : false;
        }
        private void FixedUpdate()
        {
            if(groundCheck)
                GroundCheck();
        }
        public void GetDamage(GameObject attacker, float damage)
        {
            if (state == MonsterState.Die) return;

            status.hp -= damage;
            if (status.hp <= 0)
                _controller.StateChanged(MonsterState.Die);
            else
            {
                Hit(attacker);
                _controller.StateChanged(MonsterState.Hit);
            }
        }
        void Hit(GameObject attacker)
        {
            target = attacker;
            direction = target.transform.position.x > transform.position.x ? 1 : -1;
            transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), 1f, 1f);
            if(attacker.tag == "Player")
                ParticleManager.instance.Make(1, gameObject, Vector2.zero, 0.5f);
            else
                ParticleManager.instance.Make(0, gameObject, Vector2.zero, 0.5f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            float _knockBackForce2 = 3f;
            Vector2 knockBackDirection2 = transform.position - attacker.transform.position;
            knockBackDirection2.Normalize();
            Vector2 knockBackPosition2 = new Vector2(_knockBackForce2 * Mathf.Sign(knockBackDirection2.x), 2.5f);

            GetComponent<Rigidbody2D>().AddForce(knockBackPosition2, ForceMode2D.Impulse);
        }
        public void Die()
        {
            StartCoroutine(FadeOut());
        }
        IEnumerator FadeOut()
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            float fade = 0;
            Color color = sprite.color;
            color = sprite.color;
            color.a = 1;
            sprite.color = color;
            while (fade < 1.0f)
            {
                fade += 0.01f;
                color.a -= 0.01f;
                sprite.color = color;
                yield return new WaitForSeconds(0.01f);
            }
            gameObject.SetActive(false);
            color = sprite.color;
            color.a = 0;
            sprite.color = color;
        }
    }
}
