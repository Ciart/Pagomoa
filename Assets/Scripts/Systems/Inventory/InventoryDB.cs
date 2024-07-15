using System;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;


namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public class InventoryDB
    {
        public int Gold;
        [SerializeField] public int stoneCount;
        [SerializeField] public int maxCount;
        
        const int MaxQuickSlot = 6;
        public InventoryItem[] quickSlots = new InventoryItem[MaxQuickSlot];
        
        const int MaxArtifactItems = 4;
        public InventoryItem[] artifactItems = new InventoryItem[MaxArtifactItems];
        
        const int MaxItems = 30;
        public InventoryItem[] items = new InventoryItem[MaxItems];
        
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
            this.Gold += gold;
        }
        public void Equip(Item data)
        {
            int idx = Array.FindIndex(items, element => element.item == data);
            
            if (idx != -1)
            {
                InventoryItem item = items[idx];
                
                if (item.count == 0)
                    DecreaseItemCount(data);
            }
            Inventory.Instance.ResetSlot();
        }
        
        public void AddArtifactData(Item data)
        {
            for (int i = 0; i < artifactItems.Length; i++)
            {
                if (artifactItems[i].item == null)
                {
                    artifactItems[i].item = data;
                    break;
                }
                // else if(artifactItems[artifactItems.Length].item != null)
                // {
                //     Debug.Log("꽉참");
                //     return;
                // }
            }
        }
        public void RemoveArtifactData(Item data)
        {
            int idx = Array.FindIndex(artifactItems, element => element.item == data);
            
            if (idx != -1)
            {
                artifactItems[idx].item = null;
            }
        }
        
        public void Add(Item data, int count = 1) // Item data
        {
            int idx = Array.FindIndex(items, element => element.item == data);

            if (idx != -1)
            {
                InventoryItem item = items[idx];

                item.count += count;
                for(int i = 0;  i < QuickSlotUI.instance.quickSlotsUI.Length; i++)
                {
                    if (QuickSlotUI.instance.quickSlotsUI[i].inventoryItem.item == item.item)
                    {
                        QuickSlotUI.instance.quickSlotsUI[i].itemCount.text = item.count.ToString();
                    }
                }
                EventManager.Notify(new ItemCountEvent(item.item, item.count));
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].item == null)
                    {
                        items[i].item = data;
                        items[i].count = count;
                        EventManager.Notify(new ItemCountEvent(items[i].item, items[i].count));
                        break;
                    }
                }
            }
            if (Inventory.Instance)
                Inventory.Instance.ResetSlot();
        }
        public void SellItem(Item data)
        {
            DecreaseItemCount(data);
            Gold += data.itemPrice;
            ShopUIManager.Instance.gold[0].text = Gold.ToString();
            ShopUIManager.Instance.gold[1].text = Gold.ToString();
        }
        public void DecreaseItemCount(Item data)
        {
            int idx = Array.FindIndex(items, element => element.item == data);
            
            if (idx != -1)
            {
                InventoryItem item = items[idx];
                
                if (item.count > 1)
                {
                    item.count--;
                }
                else if (item.count == 1 || item.count == 0)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i] == item)
                        {
                            items[i].item = null;
                            items[i].count = 0;
                        }
                    }
                }
            }
        }
        public void RemoveItemData(Item data)
        {
            int idx = Array.FindIndex(items, element => element.item == data);
            
            if (idx != -1)
            {
                InventoryItem item = items[idx];

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] == item)
                    {
                        items[i].item = null;
                        items[i].count = 0;

                        EventManager.Notify(new ItemCountEvent(item.item, items[i].count));
                    }
                }
            }
        }
    }
}
