using System;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public interface ISlot
    {
        public SlotType GetSlotType();
        public int GetSlotID();
        public void SetSlot(Slot targetSlot);
        public void ResetSlot();
    }
    
    public enum SlotType
    {
        None,
        Inventory,
        Quick,
        Buy,
        BuyArtifact,
        Sell,
    }
    
    public class Slot
    {
        private SlotType _type = SlotType.None;
        public SlotType GetSlotType() => _type;
        public void SetSlotType(SlotType type) => _type = type;

        private string _itemID = "";
        public string GetSlotItemID() => _itemID;
        public void SetSlotItemID(string itemID) => _itemID = itemID;
        public Item GetSlotItem() { return ResourceSystem.instance.GetItem(_itemID); }
        
        private int _itemCount = 0;
        public int GetSlotItemCount() { return _itemCount; }
        public void SetSlotItemCount(int itemCount) => _itemCount = itemCount;
    }
}