using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.CactusBoss
{
    public class RenewCactusBoss : MonoBehaviour
    {

        public float dir;
        public GameObject[] arms;
        public GameObject[] points;
        public GameObject missile;
        public Transform missileSpawnPoint;
        public GameObject footHold;

        public List<GameObject> upHammers;
        
        public float attackRange;
        public float attackRate = 1f;
        public float surgingSpeed;
        
        // [NonSerialized]
        public bool surgePoint = false;
        [NonSerialized]
        public EntityController controller;

        [NonSerialized]
        public Rigidbody2D rigid;
       
        private float _nextAttack;
        private void Awake()
        {
            controller = GetComponent<EntityController>();
            rigid = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            CheckPlayerDir();
        }

        private void Update()
        {
            CheckPlayerDir();
        }

        private void CheckPlayerDir()
        {
            Vector3 playPos = new Vector3(GameManager.player.transform.position.x, GameManager.player.transform.position.y, 
                GameManager.player.transform.position.z);
            Vector3 bossPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 distance = playPos - bossPos;

            dir = distance.x;
        }

        public bool CheckPlayerInRange()
        {
            if (dir > -attackRange && dir < attackRange)
                return true;

            return false;
        }
        public bool CheckAttackAble() => Time.time > _nextAttack;
        
        public void ApplyAttackRate()
        {
            _nextAttack = Time.time + attackRate;
        }
    }
}
