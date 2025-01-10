using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Serialization;


namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public partial class Inventory
    {
        public int gold;
        public int stoneCount;
        public int maxCount;
        
        public const int MaxQuickSlots = 6;
        public Slot[] quickItems = new Slot[MaxQuickSlots];
        
        public const int MaxArtifactSlots = 4;
        public InventorySlot[] artifactItems = new InventorySlot[MaxArtifactSlots];
        
        public const int MaxSlots = 36;
        public Slot[] inventoryItems = new Slot[MaxSlots];
        
        private void Awake()
        {
            EventManager.AddListener<AddReward>(AddReward);
            EventManager.AddListener<AddGold>(ChangeGold);
        }
        
        // 초기 인벤토리 초기화
        public void InitSlots()
        {
            for (int i = 0; i < MaxQuickSlots; i++)
            {
                quickItems[i] = new Slot();
                quickItems[i].SetSlotType(SlotType.Quick);
            } 
            
            for(int i = 0; i < MaxArtifactSlots; i++) {}

            for (int i = 0; i < MaxSlots; i++)
            {
                inventoryItems[i] = new Slot();
                inventoryItems[i].SetSlotType(SlotType.Inventory);
            }
                
        }
        
        private void ChangeGold(AddGold e) { AddGold(e.gold); }
        private void AddReward(AddReward e) { Add(e.item, e.itemCount); }
        private void AddGold(int addGold) { gold += addGold; }
        

        public void UseQuickSlotItem(int slotID)
        {
            var slot = quickItems[slotID];
            
            if (slot.GetSlotItemID() == "") return;
            
            var count = slot.GetSlotItemCount() - 1;
            
            switch (slot.GetSlotItem().type)
            {
                case ItemType.Use:
                    if (count > 0)
                    {
                        slot.GetSlotItem().DisplayUseEffect();
                        slot.SetSlotItemCount(count);
                        Debug.Log(slot.GetSlotItemCount());
                    }
                    else
                    {
                        slot.GetSlotItem().DisplayUseEffect();                        
                        slot.SetSlotItemID("");
                        slot.SetSlotItemCount(0);
                    }
                    break;
                case ItemType.Inherent:
                    slot.GetSlotItem().DisplayUseEffect();
                    break;
            }
        }
    }

    public partial class Inventory
    {
        public void Equip(Item data)
        {
            int idx = Array.FindIndex(inventoryItems, element => element.GetSlotItemID() == data.id);
            
            if (idx != -1)
            {
                var item = inventoryItems[idx];
                
                if (item.GetSlotItemCount() == 0)
                    DecreaseItemCount(data);
            }
        }
        
        public void AddArtifactData(Item data) { }
        
        public void RemoveArtifactData(Item data) { }
        
        public void Add(Item data, int count = 1)
        {
            int index = Array.FindIndex(inventoryItems, element => element.GetSlotItemID() == data.id);
        
            if (index < 0) // 아이템이 인벤토리에 존재하지 않을때
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (inventoryItems[i].GetSlotItemID() != "") continue;
                    
                    inventoryItems[i].SetSlotItemID(data.id);
                    inventoryItems[i].SetSlotItemCount(count);
                    EventManager.Notify(new ItemCountChangedEvent(inventoryItems[i].GetSlotItemID(), inventoryItems[i].GetSlotItemCount()));
                    break;
                }
            }
            else // 아이템이 인벤토리에 존재할때
            {
                var itemCount = inventoryItems[index].GetSlotItemCount() + count;
                inventoryItems[index].SetSlotItemCount(itemCount);
                
                EventManager.Notify(new ItemCountChangedEvent(inventoryItems[index].GetSlotItemID(), inventoryItems[index].GetSlotItemCount()));
            }
        }
        
        public void SellItem(Item data)
        {
            DecreaseItemCount(data);
            
            GameManager.instance.player.inventory.gold += data.price;
            UIManager.instance.UpdateGoldUI();
        }
        
        public void DecreaseItemCount(Item data)
        {
            var idx = Array.FindIndex(inventoryItems, element => element.GetSlotItemID() == data.id);
            var count = inventoryItems[idx].GetSlotItemCount();
            
            if (idx == -1) return;
            
            if (count >= 1)
            {
                inventoryItems[idx].SetSlotItemCount(count - 1);
            }

            if (count == 0)
            {
                inventoryItems[idx].SetSlotItemID("");
                inventoryItems[idx].SetSlotItemCount(0);
            }
            
            // Todo : 퀵슬롯 검색해서 인벤토리 슬롯과 동기화
            
            EventManager.Notify(new ItemCountChangedEvent(data.id, inventoryItems[idx].GetSlotItemCount()));
        }
        
        public void RemoveItemData(Item data)
        {
            var idx = Array.FindIndex(inventoryItems, element => element.GetSlotItemID() == data.id);
            
            if (idx == -1) return;
            
            inventoryItems[idx].GetSlotItem().ClearItemProperty();
            inventoryItems[idx].SetSlotItemCount(0);
            
            EventManager.Notify(new ItemCountChangedEvent(inventoryItems[idx].GetSlotItemID(), inventoryItems[idx].GetSlotItemCount()));
        }

        public void SwapSlot(int dropID, int targetID)
        {
            (inventoryItems[dropID], inventoryItems[targetID]) = (inventoryItems[targetID], inventoryItems[dropID]);
        }
        
        public int GetItemCount(Item data)
        {
            var idx = Array.FindIndex(inventoryItems, element => element.GetSlotItem() == data);

            if (idx != -1)
                return inventoryItems[idx].GetSlotItemCount();
            
            return 0;
        }
    }
}
