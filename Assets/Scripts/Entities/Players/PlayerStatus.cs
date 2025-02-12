﻿using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Entities.Players
{
    public class PlayerStatus : MonoBehaviour
    {
        [Header("# Oxygen")] public float oxygen = 100f;
        public float maxOxygen = 100f;
        public float minOxygen = 0f;

        [SerializeField] protected float _oxygenRecovery = 1;

        public float oxygenRecovery
        {
            get { return _oxygenRecovery; }
            set { _oxygenRecovery = value; }
        }

        [SerializeField] protected float _oxygenConsume = 0.01f;

        public float oxygenConsume
        {
            get { return _oxygenConsume; }
            set { _oxygenConsume = value; }
        }


        [Header("# Hungry")] public float hungry = 100f;
        public float maxHungry = 100f;
        public float minHungry = 0f;


        [SerializeField] protected float _hungryRecovery = 1;

        public float hungryRecovery
        {
            get { return _hungryRecovery; }
            set { _hungryRecovery = value; }
        }


        [SerializeField] protected float _hungryConsume = 1;

        public float hungryConsume
        {
            get { return _hungryConsume; }
            set { _hungryConsume = value; }
        }


        [Header("# Armor")][SerializeField] protected float _armor = 0;

        public float armor
        {
            get { return _armor; }
            set { _armor = value; }
        }

        [Header("# Attack")][SerializeField] protected float _attackpower = 100;

        public float attackpower
        {
            get { return _attackpower; }
            set { _attackpower = value; }
        }


        [Header("# SIght")][SerializeField] protected float _sight = 1;

        public float sight
        {
            get { return _sight; }
            set { _sight = value; }
        }


        [Header("# Speed")][SerializeField] protected float _speed = 5;

        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        [Header("Dig")][SerializeField] protected float _digSpeed = 10;

        public float digSpeed
        {
            get { return _digSpeed; }
            set { _digSpeed = value; }
        }


        [Header("CrawlUp")][SerializeField] protected float _crawlUpSpeed = 100;

        public float crawlUpSpeed
        {
            get { return _crawlUpSpeed; }
            set { _crawlUpSpeed = value; }
        }


        public UnityEvent<float, float> oxygenAlter;
        public UnityEvent<float, float> hungryAlter;

        private PlayerController _player;

        private void Awake()
        {
            _player = GetComponent<PlayerController>();
        }

        private void UpdateOxygen()
        {
            var gage = 100f;

            if (transform.position.y < World.GroundHeight && oxygen >= minOxygen)
            {
                oxygen -= Mathf.Abs(transform.position.y) * oxygenConsume * Time.deltaTime;
                gage = oxygen;
                if (oxygen < minOxygen)
                {
                    _player.entityController.Die();
                    gage = minOxygen;
                }
            }
            else if (oxygen < maxOxygen)
            {
                oxygen += Mathf.Abs(transform.position.y) * oxygenRecovery * Time.deltaTime;
                gage = oxygen;
            }
            oxygenAlter.Invoke(gage, maxOxygen);
        }

        private void FixedUpdate()
        {
            if (_player.entityController.isDead) return;
            
            UpdateOxygen();
            if (oxygen < minOxygen)
            {
                _player.entityController.TakeDamage(10);
            }
        }

        public PlayerStatus copy()
        {
            return (PlayerStatus)this.MemberwiseClone();
        }
    }
}
