using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventoryDB : MonoBehaviour
    {
        public List<InventoryItem> items = new List<InventoryItem>(new InventoryItem[30]);
        public int Gold;
        [SerializeField] public int stoneCount;
        [SerializeField] public int maxCount;
        [SerializeField] private Buy buy;

        //public UnityEvent marketCondition;

        public static InventoryDB Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(this.gameObject);
        }
        
        private void Start()
        {
            EventManager.AddListener<AddReward>(AddReward);
            EventManager.AddListener<AddGold>(ChangeGold);
        }
        
        private void ChangeGold(AddGold e)
        {
            AddGold(e.gold);
        }
        private void AddReward(AddReward e)
        {
            Add(e.item, e.itemCount);
        }
        
        private void AddGold(int gold)
        {
            Gold += gold;
        }
        public void Add(Item data, int count = 1) // Item data
        {
            var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
            var achieveItem = Achievements.Instance.AchieveMinerals.Find(achieveItem => achieveItem.item == data);

            if (inventoryItem != null)
            {
                inventoryItem.count += count;
                for(int i = 0;  i < QuickSlotItemDB.instance.quickSlots.Count; i++)
                {
                    if (QuickSlotItemDB.instance.quickSlots[i].inventoryItem.item == inventoryItem.item)
                    {
                        QuickSlotItemDB.instance.quickSlots[i].itemCount.text = inventoryItem.count.ToString();
                    }
                }
                EventManager.Notify(new ItemCountEvent(inventoryItem.item, inventoryItem.count));
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].item == null)
                    {
                        items.Insert(i, new InventoryItem(data, count));
                        items.RemoveAt(i + 1);
                        EventManager.Notify(new ItemCountEvent(items[i].item, items[i].count));
                        break;
                    }
                }
                if (data.itemType == Item.ItemType.Mineral)
                {
                    if (!Achievements.Instance.AchieveMinerals.Contains(achieveItem))
                        Achievements.Instance.AchieveMinerals.Add(new InventoryItem(data, count));
                    else
                        return;
                    //marketCondition.Invoke();
                }
            }
            if (Inventory.Instance)
                Inventory.Instance.ResetSlot();
        }
        public void Remove(Item data)
        {
            Use(data);
            Gold += data.itemPrice;
            ShopUIManager.Instance.gold[0].text = Gold.ToString();
            ShopUIManager.Instance.gold[1].text = Gold.ToString();
        }
        public void DeleteItem(Item data)
        {
            var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
            if (inventoryItem != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == inventoryItem)
                    {
                        items.RemoveAt(i);
                        items.Insert(i, new InventoryItem(null, 0));

                        EventManager.Notify(new ItemCountEvent(inventoryItem.item, items[i].count));
                    }
                }
            }
        }
        public void Use(Item data)
        {
            var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
            if (inventoryItem != null)
            {
                if (inventoryItem.count > 1)
                {
                    inventoryItem.count--;
                    QuickSlotItemDB.instance.SetCount(inventoryItem.item);
                }
                else if (inventoryItem.count == 1 || inventoryItem.count == 0)
                {
                    QuickSlotItemDB.instance.CleanSlot(inventoryItem.item);

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i] == inventoryItem)
                        {
                            items.RemoveAt(i);
                            items.Insert(i, new InventoryItem(null, 0));
                        }
                    }
                }
            }
        }
        public void Equip(Item data)
        {
            var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
            if (inventoryItem != null && inventoryItem.count == 0)
                Use(data);
            Inventory.Instance.ResetSlot();
        }

        public int FindItemCount(Item data)
        {
            var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
            if (inventoryItem != null)
                return inventoryItem.count;
            else
                return 0;
        }
    }
}
