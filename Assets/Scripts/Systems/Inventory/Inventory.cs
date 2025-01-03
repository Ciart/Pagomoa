using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Serialization;


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
        
        public const int MaxArtifactSlots = 4;
        public InventorySlot[] artifactItems = new InventorySlot[MaxArtifactSlots];
        
        public const int MaxSlots = 36;
        public InventorySlot[] inventorySlots = new InventorySlot[MaxSlots];
        
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
            int idx = Array.FindIndex(inventorySlots, element => element.GetSlotItem() == data);
            
            if (idx != -1)
            {
                InventorySlot item = inventorySlots[idx];
                
                if (item.GetSlotItemCount() == 0)
                    DecreaseItemCount(data);
            }
        }
        
        public void AddArtifactData(Item data)
        {
            for (int i = 0; i < artifactItems.Length; i++)
            {
                if (artifactItems[i].GetSlotItem().type == ItemType.None)
                {
                    artifactItems[i].SetSlotItem(data);
                    break;
                }
            }
        }
        
        public void RemoveArtifactData(Item data)
        {
            int idx = Array.FindIndex(artifactItems, element => element.GetSlotItem().id == data.id);
            
            if (idx != -1)
            {
                artifactItems[idx].GetSlotItem().ClearItemProperty();
            }
        }
        
        public void Add(Item data, int count = 1)
        {
            int index = Array.FindIndex(inventorySlots, element => element.GetSlotItem().id == data.id);
        
            if (index < 0) // 아이템이 인벤토리에 존재하지 않을때
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (inventorySlots[i].GetSlotItem().id != "") continue;
                    
                    inventorySlots[i].SetSlotItem(data);
                    inventorySlots[i].SetSlotItemCount(count);
                    EventManager.Notify(new ItemCountChangedEvent(inventorySlots[i].GetSlotItem(), inventorySlots[i].GetSlotItemCount()));
                    break;
                }
            }
            else // 아이템이 인벤토리에 존재할때
            {
                var itemCount = inventorySlots[index].GetSlotItemCount() + count;
                inventorySlots[index].SetSlotItemCount(itemCount);
                
                EventManager.Notify(new ItemCountChangedEvent(inventorySlots[index].GetSlotItem(), inventorySlots[index].GetSlotItemCount()));
            }
        }
        
        public void SellItem(Item data)
        {
            DecreaseItemCount(data);
            
            GameManager.instance.player.inventory.Gold += data.price;
            UIManager.instance.UpdateGoldUI();
        }
        
        public void DecreaseItemCount(Item data)
        {
            var idx = Array.FindIndex(inventorySlots, element => element.GetSlotItem().id == data.id);
            var count = inventorySlots[idx].GetSlotItemCount();
            
            if (idx == -1) return;
            
            if (count >= 1)
            {
                inventorySlots[idx].SetSlotItemCount(count - 1);
            }

            if (count == 0)
            {
                inventorySlots[idx].GetSlotItem().ClearItemProperty();
                inventorySlots[idx].ResetSlot();
            }
                
            EventManager.Notify(new ItemCountChangedEvent(data, inventorySlots[idx].GetSlotItemCount()));
        }
        
        public void RemoveItemData(Item data)
        {
            var idx = Array.FindIndex(inventorySlots, element => element.GetSlotItem().id == data.id);
            
            if (idx == -1) return;
            
            inventorySlots[idx].GetSlotItem().ClearItemProperty();
            inventorySlots[idx].SetSlotItemCount(0);
            
            EventManager.Notify(new ItemCountChangedEvent(inventorySlots[idx].GetSlotItem(), inventorySlots[idx].GetSlotItemCount()));
        }

        public void SwapSlot(int dropID, int targetID)
        {
            var toTargetSlot = dropID;
            var toDropSlot = targetID;
            (inventorySlots[dropID], inventorySlots[targetID]) = (inventorySlots[targetID], inventorySlots[dropID]);
            inventorySlots[dropID].id = toTargetSlot;
            ;inventorySlots[targetID].id = toDropSlot;
        }
        
        public int GetItemCount(Item data)
        {
            var idx = Array.FindIndex(inventorySlots, element => element.GetSlotItem() == data);

            if (idx != -1)
                return inventorySlots[idx].GetSlotItemCount();
            
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
                        item.DisplayUseEffect();
                    }
                    break;
                case ItemType.Inherent:
                    item.DisplayUseEffect();
                    break;
                default:
                    return;
            }
        }
    }
}
