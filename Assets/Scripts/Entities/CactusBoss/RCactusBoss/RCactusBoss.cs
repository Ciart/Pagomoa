using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RCactusBoss : MonoBehaviour
    {
        public float dir;
        public GameObject[] hammers;
        public Transform[] initialPos = new Transform[2];

        public float attackRange;
        public float attackRate = 1f;

        public float hammerDownSpeed;

        // public enum State
        // {
        //     Hammer,
        //     Spin,
        //     Smash
        // }
        //
        // public State state;
        
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
        }
        private void CheckPlayerDir()
        {
            Vector3 playPos = new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y, 
                GameManager.instance.player.transform.position.z);
            Vector3 bossPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 distance = playPos - bossPos;
        
            dir = distance.x;
            if(dir > 0)
                _spriteRenderer.flipX = true;
            else if(dir < 0)
                _spriteRenderer.flipX = false;
        }
        public bool CheckPlayerInRange()
        {
            if (dir > -attackRange && dir < attackRange)
                return true;

            return false;
        }
        private void InstantiateHammer()
        {
            EntityController? leftHammer = EntityManager.instance.Spawn("LeftHammer", initialPos[0].position);
            EntityController? rightHammer = EntityManager.instance.Spawn("RightHammer", initialPos[1].position);
            
            leftHammer!.transform.SetParent(transform);
            rightHammer!.transform.SetParent(transform);

            hammers[0] = leftHammer!.gameObject;
            hammers[1] = rightHammer!.gameObject;
        }
    }
}
