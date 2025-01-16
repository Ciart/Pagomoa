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
            Attack,
            Die               // ����
        }
        private void Awake()
        {
            status = GetComponent<MonsterStatus>();    
        }

        void GroundCheck()
        {
            isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, LayerMask.GetMask("Platform")) ? true : false;
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
