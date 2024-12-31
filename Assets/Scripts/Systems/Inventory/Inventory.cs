using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;


namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public class Inventory
    {
        public int Gold;
        [SerializeField] public int stoneCount;
        [SerializeField] public int maxCount;
        
        public const int MaxQuickItems = 6;
        private Item[] _quickItems = new Item[MaxQuickItems];
        
        public const int MaxArtifactItems = 4;
        public InventorySlot[] artifactItems = new InventorySlot[MaxArtifactItems];
        
        public const int MaxItems = 36;
        public InventorySlot[] items = new InventorySlot[MaxItems];
        
        private void Start()
        {
            EventManager.AddListener<AddReward>(AddReward);
            EventManager.AddListener<AddGold>(ChangeGold);
        }
        
        private void ChangeGold(AddGold e) { AddGold(e.gold); }
        private void AddReward(AddReward e) { Add(e.item, e.itemCount); }
        private void AddGold(int gold) { Gold += gold; }
        
        public void Equip(Item data)
        {
            int idx = Array.FindIndex(items, element => element.GetSlotItem() == data);
            
            if (idx != -1)
            {
                InventorySlot item = items[idx];
                
                if (item.GetSlotItemCount() == 0)
                    DecreaseItemCount(data);
            }
        }
        
        public void AddArtifactData(Item data)
        {
            for (int i = 0; i < artifactItems.Length; i++)
            {
                if (artifactItems[i].GetSlotItem() == null)
                {
                    artifactItems[i].SetSlotItem(data);
                    break;
                }
                // else if(artifactItems[artifactItems.Length].item != null)
                // {
                //     Debug.Log("꽉참");
                //     return;
                // } 수정 필요
            }
        }
        
        public void RemoveArtifactData(Item data)
        {
            int idx = Array.FindIndex(artifactItems, element => element.GetSlotItem() == data);
            
            if (idx != -1)
            {
                artifactItems[idx].SetSlotItem(null);
            }
        }
        
        public void Add(Item data, int count = 1) // Item data
        {
            int idx = Array.FindIndex(items, element => element.GetSlotItem() == data);

            if (idx != -1)
            {
                ref var item = ref items[idx];
                var addCount = item.GetSlotItemCount() + count; 
                item.SetSlotItemCount(addCount);
                
                EventManager.Notify(new ItemCountChangedEvent(item.GetSlotItem(), item.GetSlotItemCount()));
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetSlotItem() == null)
                    {
                        items[i].SetSlotItem(data);
                        items[i].SetSlotItemCount(count);
                        EventManager.Notify(new ItemCountChangedEvent(items[i].GetSlotItem(), items[i].GetSlotItemCount()));
                        break;
                    }
                }
            }
        }
        
        public void SellItem(Item data)
        {
            var playerGold = UIManager.instance.shopUI.playerGold;
            
            DecreaseItemCount(data);
            Gold += data.price;

            foreach (var target in playerGold)
            {
                target.text = Gold.ToString();
            }
        }
        
        public void DecreaseItemCount(Item data)
        {
            var idx = Array.FindIndex(items, element => element.GetSlotItem() == data);
            var count = items[idx].GetSlotItemCount();
            
            
            if (idx != -1)
            {
                if (count > 1)
                {
                    items[idx].SetSlotItemCount(count - 1);
                }
                else if (count >= 0)
                {
                    items[idx].SetSlotItem(null);
                    items[idx].SetSlotItemCount(0);
                }
                
                EventManager.Notify(new ItemCountChangedEvent(data, items[idx].GetSlotItemCount()));
            }
        }
        
        public void RemoveItemData(Item data)
        {
            var idx = Array.FindIndex(items, element => element.GetSlotItem() == data);
            
            if (idx == -1) return;
            
            items[idx].SetSlotItem(null);
            items[idx].SetSlotItemCount(0);
            
            EventManager.Notify(new ItemCountChangedEvent(items[idx].GetSlotItem(), items[idx].GetSlotItemCount()));
        }

        public void SwapSlot(int a, int b)
        {
            (items[a], items[b]) = (items[b], items[a]);
            EventManager.Notify(new ItemCountChangedEvent(items[a].GetSlotItem(), items[a].GetSlotItemCount()));
            EventManager.Notify(new ItemCountChangedEvent(items[b].GetSlotItem(), items[b].GetSlotItemCount()));
        }
        
        public int GetItemCount(Item data)
        {
            var idx = Array.FindIndex(items, element => element.GetSlotItem() == data);

            if (idx != -1)
                return items[idx].GetSlotItemCount();
            
            return 0;
        }
        
        public Item GetQuickItem(int id)
        {
            return _quickItems[id];
        }
        
        public void SetQuickItem(int id, Item item)
        {
            _quickItems[id] = item;
            EventManager.Notify(new QuickSlotChangedEvent(_quickItems));
        }
        
        public void SwapQuickSlot(int a, int b)
        {
            (_quickItems[a], _quickItems[b]) = (_quickItems[b], _quickItems[a]);
            EventManager.Notify(new QuickSlotChangedEvent(_quickItems));
        }

        public void UseQuickSlotItem(int index)
        {
            var player = GameManager.instance.player;
            var item = _quickItems[index];
            
            switch (item.type)
            {
                case ItemType.Use:
                    if (player.inventory.GetItemCount(item) != 0)
                    {
                        DecreaseItemCount(item);
                        item.Use();
                    }
                    break;
                case ItemType.Inherent:
                    item.Use();
                    break;
                default:
                    return;
            }
        }
    }
}
