using System;
using Maps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Player
{
    public class Status : MonoBehaviour
    {
        [Header("# Oxygen")]
        public float oxygen = 100f;
        //public float oxygenConsume = 0.001f;
        public float maxOxygen = 100f;
        public float minOxygen = 0f;

        protected float _extraMaxOxygen = 0;
        protected float _extraMaxOxygenPercentage = 0;
        public float extramaxOxygen { get { return _extraMaxOxygen; } set { _extraMaxOxygen = value; } }
        public float extraMaxOxygenPercentage { get { return _extraMaxOxygenPercentage; } set { _extraMaxOxygenPercentage = value; } }

        [SerializeField] protected float _oxygenRecovery = 1;
        protected float _extraOxygenRecovery = 0;
        protected float _extraOxygenRecoveryPercentage = 0;
        public float oxygenRecovery { get { return _oxygenRecovery; } set { _oxygenRecovery = value; } }
        public float extraOxygenRecovery { get { return _extraOxygenRecovery; } set { _extraOxygenRecovery = value; } }
        public float extraOxygenRecoveryPercentage { get { return _extraOxygenRecoveryPercentage; } set { _extraOxygenRecoveryPercentage = value; } }

        [SerializeField] protected float _oxygenConsume = 0.001f;
        protected float _extraOxygenConsume = 0;
        protected float _extraOxygenConsumePercentage = 0;
        public float oxygenConsume { get { return _oxygenConsume; } set { _oxygenConsume = value; } }
        public float extraOxygenConsume { get { return _extraOxygenConsume; } set { _extraOxygenConsume = value; } }
        public float extraOxygenConsumePercentage { get { return _extraOxygenConsumePercentage; } set { _extraOxygenConsumePercentage = value; } }


        [Header("# Hungry")]
        public float hungry = 100f;
        public float maxHungry = 100f;
        public float minHungry = 0f;

        protected float _extraHungry = 0;
        protected float _extraHungryPercentage = 0;
        public float extraHungry { get { return _extraHungry; } set { _extraHungry = value; } }
        public float extraHungryPercentage { get { return _extraHungryPercentage; } set { _extraHungryPercentage = value; } }

        [SerializeField] protected float _hungryRecovery = 1;
        protected float _extraHungryRecovery = 0;
        protected float _extraHungryRecoveryPercentage = 0;
        public float hungryRecovery { get { return _hungryRecovery; } set { _hungryRecovery = value; } }
        public float extraHungryRecovery { get { return _extraHungryRecovery; } set { _extraHungryRecovery = value; } }
        public float extraHungryRecoveryPercentage { get { return _extraHungryRecoveryPercentage; } set { _extraHungryRecoveryPercentage = value; } }


        [SerializeField] protected float _hungryConsume = 1;
        protected float _extraHungryConsume = 0;
        protected float _extraHungryConsumePercentage = 0;
        public float hungryConsume { get { return _hungryConsume; } set { _hungryConsume = value; } }
        public float extraHungryConsume { get { return _extraHungryConsume; } set { _extraHungryConsume = value; } }
        public float extraHungryConsumePercentage { get { return _extraHungryConsumePercentage; } set { _extraHungryConsumePercentage = value; } }


        [Header("# Armor")]
        [SerializeField] protected float _armor = 0;
        protected float _extraArmor = 0;
        protected float _extraArmorPercentage = 0;
        public float armor { get { return _armor; } set { _armor = value; } }
        public float extraArmor { get { return _extraArmor; } set { _extraArmor = value; } }
        public float extraArmorPercentage { get { return _extraArmorPercentage; } set { _extraArmorPercentage = value; } }

        [Header("# Attack")]
        [SerializeField] protected float _attackpower = 100;
        protected float _extraAttackpower = 0;
        protected float _extraAttackpowerPercentage = 0;
        public float attackpower { get { return _attackpower; } set { _attackpower = value; } }
        public float extraAttackpower { get { return _extraAttackpower; } set { _extraAttackpower = value; } }
        public float extraAttackpowerPercentage { get { return _extraAttackpowerPercentage; } set { _extraAttackpowerPercentage = value; } }


        [Header("# SIght")]
        [SerializeField] protected float _sight = 1;
        protected float _extraSIght = 0;
        protected float _extraSIghtPercentage = 0;
        public float sight { get { return _sight; } set { _sight = value; } }
        public float extraSIght { get { return _extraSIght; } set { _extraSIght = value; } }
        public float extraSIghtPercentage { get { return _extraSIghtPercentage; } set { _extraSIghtPercentage = value; } }

        [SerializeField] protected float _inSight = 1;
        protected float _extraInSIght = 0;
        protected float _extraInSIghtPercentage = 0;
        public float inSight { get { return _inSight; } set { _inSight = value; } }
        public float extraInSIght { get { return _extraInSIght; } set { _extraInSIght = value; } }
        public float extraInSIghtPercentage { get { return _extraInSIghtPercentage; } set { _extraInSIghtPercentage = value; } }

        [SerializeField] protected float _outSight = 1;
        protected float _extraOutSight = 0;
        protected float _extraOutSightPercentage = 0;
        public float outSight { get { return _outSight; } set { _outSight = value; } }
        public float extraOutSight { get { return _extraOutSight; } set { _extraOutSight = value; } }
        public float extraOutSightPercentage { get { return _extraOutSightPercentage; } set { _extraOutSightPercentage = value; } }

        [Header("# Speed")]
        [SerializeField] protected float _speed = 5;
        protected float _extraSpeed = 0;
        protected float _extraSpeedPercentage = 0;
        public float speed { get { return _speed; } set { _speed = value; } }
        public float extraSpeed { get { return _extraSpeed; } set { _extraSpeed = value; } }
        public float extraSpeedPercentage { get { return _extraSpeedPercentage; } set { _extraSpeedPercentage = value; } }


        [SerializeField] protected float _inSpeed = 5;
        protected float _extraInSpeed = 0;
        protected float _extraInSpeedPercentage = 0;
        public float inSpeed { get { return _inSpeed; }set { _inSpeed = value; } }
        public float extraInSpeed { get { return _extraInSpeed; } set { _extraInSpeed = value; } }
        public float extraInSpeedPercentage { get { return _extraInSpeedPercentage; } set { _extraInSpeedPercentage = value; } }


        [SerializeField] protected float _outSpeed = 5;
        protected float _extraOutSpeed = 0;
        protected float _extraOutSpeedPercentage = 0;
        public float outSpeed { get { return _outSpeed; } set { _outSpeed = value; } }
        public float extraOutSpeed { get { return _extraOutSpeed; } set { _extraOutSpeed = value; } }
        public float extraOutSpeedPercentage { get { return _extraOutSpeedPercentage; } set { _extraOutSpeedPercentage = value; } }


        [Header("Dig")]

        [SerializeField] protected float _digSpeed = 100;
        protected float _extraDigSpeed = 0;
        protected float _extraDigSpeedPercentage = 0;
        public float digSpeed { get { return _digSpeed; } set { _digSpeed = value; } }
        public float extraDigSpeed { get { return _extraDigSpeed; } set { _extraDigSpeed = value; } }
        public float extraDigSpeedPercentage { get { return _extraDigSpeedPercentage; } set { _extraDigSpeedPercentage = value; } }

        
        [Header("CrawlUp")]

        [SerializeField] protected float _crawlUpSpeed = 100;
        protected float _extraCrawlUpSpeed = 0;
        protected float _extraCrawlUpSpeedPercentage = 0;
        public float crawlUpSpeed { get { return _crawlUpSpeed; } set { _crawlUpSpeed = value; } }
        public float extraCrawlUpSpeed { get { return _extraCrawlUpSpeed; } set { _extraCrawlUpSpeed = value; } }
        public float extraCrawlUpSpeedPercentage { get { return _extraCrawlUpSpeedPercentage; } set { _extraCrawlUpSpeedPercentage = value; } }




        public UnityEvent<float, float> oxygenAlter;
        public UnityEvent<float, float> hungryAlter;

        private WorldManager _worldManager;

        private void Awake()
        {
            _worldManager = WorldManager.Instance;
        }

        private void UpdateOxygen()
        {
            if (transform.position.y < _worldManager.World.GroundHeight && oxygen >= minOxygen)
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
            UpdateOxygen();
        }

        public Status copy()
        {
            return (Status)this.MemberwiseClone();
        }
    }
}
