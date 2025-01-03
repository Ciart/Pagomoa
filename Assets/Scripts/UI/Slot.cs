using System;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public enum SlotType
    {
        Default = 0,
        Inventory,
        Sell,
        Buy,
        BuyArtifact,
    }
    
    
    [RequireComponent(typeof(Image))]
    public class Slot : MonoBehaviour
    {
        [SerializeField] private SlotType _slotType = SlotType.Default;
        public SlotType GetSlotType() => _slotType;
        public void SetSlotType(SlotType type) => _slotType = type;
        
        [SerializeField] private Item _item;
        public Item GetSlotItem() => _item;
        public void SetSlotItem(Item item) => _item.SetItem(item);
        
        private int _itemCount = 0;
        public int GetSlotItemCount() => _itemCount;
        public void SetSlotItemCount(int count) => _itemCount = count;

        public virtual void SetSlot(Slot slot) { }
    }
}
