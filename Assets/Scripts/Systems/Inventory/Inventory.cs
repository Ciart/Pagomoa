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

            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var quickSlotUI = UIManager.instance.quickSlotUI;
            var count = slot.GetSlotItemCount() - 1;
            
            switch (slot.GetSlotItem().type)
            {
                case ItemType.Use:
                    if (count > 0)
                    {
                        slot.GetSlotItem().DisplayUseEffect();
                        slot.SetSlotItemCount(count);
                    }
                    else
                    {
                        slot.GetSlotItem().DisplayUseEffect();                        
                        slot.SetSlotItemID("");
                        slot.SetSlotItemCount(0);
                    }
                    quickSlotUI.UpdateQuickSlot();
                    inventoryUI.UpdateInventorySlotByQuickSlotID(slotID);
                    break;
                case ItemType.Inherent:
                    slot.GetSlotItem().DisplayUseEffect();
                    break;
            }
        }
    }

    public partial class Inventory
    {
        public void Equip(Item data) { }
        
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
        
        public void SellItem(Item data) { }
        
        public void DecreaseItemBySlotID(InventorySlot targetSlot)
        {
            var count = inventoryItems[targetSlot.id].GetSlotItemCount() - 1;
            
            if (count >= 1)
            {
                inventoryItems[targetSlot.id].SetSlotItemCount(count);
            }
            else if (count == 0)
            {
                inventoryItems[targetSlot.id].SetSlotItemID("");
                inventoryItems[targetSlot.id].SetSlotItemCount(0);
            }

            if (targetSlot.referenceSlotID != -1)
            {
                quickItems[targetSlot.referenceSlotID].SetSlotItemCount(count);
                
                if (quickItems[targetSlot.referenceSlotID].GetSlotItemCount() == 0)
                {
                    quickItems[targetSlot.referenceSlotID].SetSlotItemID("");
                    quickItems[targetSlot.referenceSlotID].SetSlotItemCount(0);
                }
            }
            
            EventManager.Notify(new ItemCountChangedEvent(
                inventoryItems[targetSlot.id].GetSlotItemID(), 
                inventoryItems[targetSlot.id].GetSlotItemCount()));
        }
        
        public void DecreaseItemBySlotID(InventorySlot targetSlot, int mineralCount)
        {
            if (inventoryItems[targetSlot.id].GetSlotItem().type != ItemType.Mineral) return;
            var count = inventoryItems[targetSlot.id].GetSlotItemCount() - mineralCount;
            
            if (count >= 1)
            {
                inventoryItems[targetSlot.id].SetSlotItemCount(count);
            }
            else if (count == 0)
            {
                inventoryItems[targetSlot.id].SetSlotItemID("");
                inventoryItems[targetSlot.id].SetSlotItemCount(0);
            }

            if (targetSlot.referenceSlotID != -1)
            {
                quickItems[targetSlot.referenceSlotID].SetSlotItemCount(count);
            }
            if (quickItems[targetSlot.referenceSlotID].GetSlotItemCount() == 0)
            {
                quickItems[targetSlot.referenceSlotID].SetSlotItemID("");
                quickItems[targetSlot.referenceSlotID].SetSlotItemCount(0);
            }
            EventManager.Notify(new ItemUsedEvent(
                inventoryItems[targetSlot.id].GetSlotItem(),
                mineralCount));
            
            EventManager.Notify(new ItemCountChangedEvent(
                inventoryItems[targetSlot.id].GetSlotItemID(), 
                inventoryItems[targetSlot.id].GetSlotItemCount()));
        }
        
        public void RemoveItemBySlotID(InventorySlot targetSlot)
        {
            inventoryItems[targetSlot.id].SetSlotItemID("");
            inventoryItems[targetSlot.id].SetSlotItemCount(0);

            if (targetSlot.referenceSlotID != -1)
            {
                quickItems[targetSlot.referenceSlotID].SetSlotItemID("");
                quickItems[targetSlot.referenceSlotID].SetSlotItemCount(0);
                targetSlot.referenceSlotID = -1;
            }
            
            EventManager.Notify(new ItemCountChangedEvent(
                inventoryItems[targetSlot.id].GetSlotItemID(), 
                inventoryItems[targetSlot.id].GetSlotItemCount()));
        }

        public void RemoveItemByItemID(string itemID)
        {
            foreach (var slot in inventoryItems)
            {
                if (slot.GetSlotItemID() == itemID)
                {
                    slot.SetSlotItemID("");
                    slot.SetSlotItemCount(0);
                    
                    EventManager.Notify(new ItemCountChangedEvent(
                        slot.GetSlotItemID(), 
                        slot.GetSlotItemCount()));
                    break;
                }
            }
        }

        public Slot FindInventorySlotByID(ISlot targetSlot)
        {
            return inventoryItems[targetSlot.GetSlotID()];
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
