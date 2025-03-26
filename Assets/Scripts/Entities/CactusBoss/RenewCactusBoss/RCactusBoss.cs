using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class RCactusBoss : MonoBehaviour
    {
        public int patternCount;
        public float dir;
        public GameObject[] hammers;
        public Transform[] initialPos = new Transform[2];
        public float attackRange;
        public float attackRate = 1f;

        public Transform[] leftArms;
        public Transform[] rightArms;
      
        // 추후 random 값을 이용해 State 결정
        public enum State
        {
            Hammer = 0,
            Spin = 1,
            Smash = 2,
            Idle = 3,
            Groggy = 4
        }
        
        public State state;
        
        private SpriteRenderer _spriteRenderer;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            InstantiateHammer();
            CheckPlayerDir();
        }
        private void Update()
        {
            CheckPlayerDir();
            UpdateArmTransforms(leftArms, hammers[0].transform.position, true);
            UpdateArmTransforms(rightArms, hammers[1].transform.position);
        }
        private void CheckPlayerDir()
        {
            Vector3 playPos = new Vector3(Game.Instance.player.transform.position.x, Game.Instance.player.transform.position.y, 
                Game.Instance.player.transform.position.z);
            Vector3 bossPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 distance = playPos - bossPos;
        
            dir = distance.x;
            if(dir > 0)
                _spriteRenderer.flipX = true;
            else if(dir < 0)
                _spriteRenderer.flipX = false;
        }
        private void UpdateArmTransforms(Transform[] arms, Vector3 target, bool invertRotation = false)
        {
            Vector3 direction = (target - arms[0].position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (invertRotation)
                angle -= 180;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            for (int i = 0; i < arms.Length; i++)
            {
                arms[i].rotation = Quaternion.Slerp(arms[i].rotation, targetRotation, Time.deltaTime * 5f);
            }
            
            int totalArms = arms.Length;
            for (int i = 0; i < totalArms - 1; i++)
            {
                arms[i].position = Vector3.Lerp(target, arms[totalArms - 1].position, (i + 1f) / totalArms);
            }
        }
        public bool CheckPlayerInRange()
        {
            if (dir > -attackRange && dir < attackRange)
                return true;

            return false;
        }
        private void InstantiateHammer()
        {
            EntityController? leftHammer = Game.Instance.Entity.Spawn("LeftHammer", initialPos[0].position);
            EntityController? rightHammer = Game.Instance.Entity.Spawn("RightHammer", initialPos[1].position);
            
            leftHammer!.transform.SetParent(transform);
            rightHammer!.transform.SetParent(transform);

            hammers[0] = leftHammer!.gameObject;
            hammers[1] = rightHammer!.gameObject;
        }
    }
}
