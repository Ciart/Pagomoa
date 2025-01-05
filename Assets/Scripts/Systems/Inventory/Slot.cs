using System;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public enum SlotType
    {
        None,
        Inventory,
        Quick,
        Buy,
        BuyArtifact,
        Sell,
    }
    
    public class Slot : MonoBehaviour
    {
        [SerializeField] private SlotType _type;
        public SlotType GetSlotType() => _type;
        protected void SetSlotType(SlotType type) => _type = type;

        [SerializeField] private Item? _item;
        public Item GetSlotItem() { return _item; }
        public void SetSlotItem(Item item) => _item = item;
        
        [SerializeField] private int _itemCount;
        public int GetSlotItemCount() { return _itemCount; }
        public void SetSlotItemCount(int itemCount) => _itemCount = itemCount;

        public virtual void SetSlot(Slot slot) { }
    }
}