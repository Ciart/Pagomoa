using System;
using Worlds;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Player
{
    public class Status : MonoBehaviour
    {
        [Header("# Oxygen")]
        public float oxygen = 100f;
        public float maxOxygen = 100f;
        public float minOxygen = 0f;


        [SerializeField] protected float _oxygenRecovery = 1;
        public float oxygenRecovery { get { return _oxygenRecovery; } set { _oxygenRecovery = value; } }

        [SerializeField] protected float _oxygenConsume = 0.001f;
        public float oxygenConsume { get { return _oxygenConsume; } set { _oxygenConsume = value; } }


        [Header("# Hungry")]
        public float hungry = 100f;
        public float maxHungry = 100f;
        public float minHungry = 0f;


        [SerializeField] protected float _hungryRecovery = 1;
        public float hungryRecovery { get { return _hungryRecovery; } set { _hungryRecovery = value; } }


        [SerializeField] protected float _hungryConsume = 1;
        public float hungryConsume { get { return _hungryConsume; } set { _hungryConsume = value; } }


        [Header("# Armor")]
        [SerializeField] protected float _armor = 0;
        public float armor { get { return _armor; } set { _armor = value; } }

        [Header("# Attack")]
        [SerializeField] protected float _attackpower = 100;
        public float attackpower { get { return _attackpower; } set { _attackpower = value; } }


        [Header("# SIght")]
        [SerializeField] protected float _sight = 1;
        public float sight { get { return _sight; } set { _sight = value; } }


        [Header("# Speed")]
        [SerializeField] protected float _speed = 5;
        public float speed { get { return _speed; } set { _speed = value; } }

        [Header("Dig")]

        [SerializeField] protected float _digSpeed = 100;
        public float digSpeed { get { return _digSpeed; } set { _digSpeed = value; } }

        
        [Header("CrawlUp")]

        [SerializeField] protected float _crawlUpSpeed = 100;
        public float crawlUpSpeed { get { return _crawlUpSpeed; } set { _crawlUpSpeed = value; } }




        public UnityEvent<float, float> oxygenAlter;
        public UnityEvent<float, float> hungryAlter;

        private WorldManager _worldManager;

        private void Awake()
        {
            _worldManager = WorldManager.instance;
        }

        private void UpdateOxygen()
        {
            if (transform.position.y < _worldManager.world.groundHeight && oxygen >= minOxygen)
            {
                oxygen -= Mathf.Abs(transform.position.y) * oxygenConsume * Time.deltaTime;

                if (oxygen < minOxygen)
                {
                    oxygen = minOxygen;
                }
            }
            else if (oxygen < maxOxygen)
            {
                oxygen += Mathf.Abs(transform.position.y) * oxygenConsume * Time.deltaTime;
            }

            oxygenAlter.Invoke(oxygen, maxOxygen);
        }
        
        private void Update()
        {
            // UpdateOxygen();
        }

        public Status copy()
        {
            return (Status)this.MemberwiseClone();
        }
    }
}
