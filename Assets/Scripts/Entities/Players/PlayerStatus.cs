using System.Collections.Generic;
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

        public bool isDie = false;

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


        [Header("# Armor")] [SerializeField] protected float _armor = 0;

        public float armor
        {
            get { return _armor; }
            set { _armor = value; }
        }

        [Header("# Attack")] [SerializeField] protected float _attackpower = 100;

        public float attackpower
        {
            get { return _attackpower; }
            set { _attackpower = value; }
        }


        [Header("# SIght")] [SerializeField] protected float _sight = 1;

        public float sight
        {
            get { return _sight; }
            set { _sight = value; }
        }


        [Header("# Speed")] [SerializeField] protected float _speed = 5;

        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        [Header("Dig")] [SerializeField] protected float _digSpeed = 10;

        public float digSpeed
        {
            get { return _digSpeed; }
            set { _digSpeed = value; }
        }


        [Header("CrawlUp")] [SerializeField] protected float _crawlUpSpeed = 100;

        public float crawlUpSpeed
        {
            get { return _crawlUpSpeed; }
            set { _crawlUpSpeed = value; }
        }


        public UnityEvent<float, float> oxygenAlter;
        public UnityEvent<float, float> hungryAlter;

        private WorldManager _worldManager;

        private void Awake()
        {
            _worldManager = WorldManager.instance;
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
                    Die();
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

        private void Die()
        {
            var inventory = GameManager.instance.player.inventory;
            inventory.gold = Mathf.FloorToInt(inventory.gold * 0.9f);
            
            LoseMoney(0.1f);
            LoseItem(ItemType.Mineral, 0.5f);
            
            TimeManager.instance.SkipToNextDay();

            Respawn();
        }

        private void Respawn()
        {
            GameManager.instance.player.transform.position = FindObjectOfType<SpawnPoint>().transform.position;
            
            oxygen = maxOxygen;
            isDie = false;
        }

        private void LoseMoney(float percentage)
        {
            var inventory = GameManager.instance.player.inventory;
            inventory.gold = (int)(inventory.gold * (1 - percentage));
        }

        // TODO : 사망 시 아이템 제거 기능 잠금 
        private void LoseItem(ItemType itemType, float probabilty)
        {
            var inventory = GameManager.instance.player.inventory;

            /*List<string> deleteItems = new List<string>();*/
            
            foreach (var item in inventory.GetSlots(SlotType.Inventory))
            {
                if (item.GetSlotItem().id == "") continue;

                var rand = Random.Range(0, 101) * 0.01f;
                if (probabilty < rand)
                {
                    Debug.Log("item not Losted by" + probabilty + "<" + rand);
                    continue;
                }

                if (item.GetSlotItem().type == itemType)
                {
                    for (int i = 0; i < item.GetSlotItemCount(); i++)
                    {
                        var entity = Instantiate(WorldManager.instance.itemEntity, transform.position,
                            Quaternion.identity);
                        entity.Item = item.GetSlotItem();
                        entity.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5, 5), 100));
                    }

                    /*deleteItems.Add(item.GetSlotItem().id);*/
                }
            }

            /*var count = deleteItems.Count;
            for (int i = 0; i < count; i++)
                inventory.RemoveInventoryItem(ResourceSystem.instance.GetItem(deleteItems[i]));*/
        }

        private void NextDay()
        {
            var timeManager = TimeManager.instance;
            
            timeManager.SetTime(6, 0);
            timeManager.AddDay(1);
        }

        private void FixedUpdate()
        {
            UpdateOxygen();
            if (oxygen < minOxygen && !isDie)
            {
                GetComponent<EntityController>().TakeDamage(10);
            }
        }

        public PlayerStatus copy()
        {
            return (PlayerStatus)this.MemberwiseClone();
        }
    }
}
