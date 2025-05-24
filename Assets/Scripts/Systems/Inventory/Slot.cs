using System;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public interface ISlot
    {
        public SlotType GetSlotType();
        public int GetSlotID();
    }
    
    public enum SlotType
    {
        None,
        Inventory,
        Quick,
        Buy,
        BuyArtifact,
        Sell,
        Artifact,
    }
    
    public class Slot
    {
        private SlotType _type = SlotType.None;
        private string _itemID = "";
        private int _itemCount = 0;
        private ItemType _itemType = ItemType.None;
        
        public SlotType GetSlotType() => _type;
        public void SetSlotType(SlotType type) => _type = type;

        public ItemType GetItemType() { return _itemType; }
        
        public string GetSlotItemID() => _itemID;

        public void SetSlotItemID(string itemID)
        {
            _itemID = itemID;
            
            if (_itemID == "")
            {
                _itemType = ItemType.None;
                return;
            }
            
            _itemType = ResourceSystem.Instance.GetItem(_itemID).type;
        }
        public Item? GetSlotItem() {
            if (string.IsNullOrEmpty(_itemID)) return null;

            return ResourceSystem.Instance.GetItem(_itemID);
        }
        
        
        public int GetSlotItemCount() { return _itemCount; }
        public void SetSlotItemCount(int itemCount) => _itemCount = itemCount;
    }
}
