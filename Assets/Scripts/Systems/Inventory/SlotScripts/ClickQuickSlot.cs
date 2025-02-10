using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ClickQuickSlot : MonoBehaviour
        , IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        private QuickSlotUI quickSlotUI => GetComponent<QuickSlotUI>();
        
        public void OnPointerDown(PointerEventData eventData)
        {
            var inventory = Game.Instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Quick, quickSlotUI.slotID);

            if (targetSlot == null) return;
            
            UIManager.instance.quickUI.SelectQuickSlot(quickSlotUI.GetSlotID() + 1);;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var inventory = Game.Instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Quick, quickSlotUI.slotID);
            
            if (targetSlot == null) return;
            
            DragItem.instance.DragSetImage(targetSlot.GetSlotItem().sprite);
            DragItem.instance.transform.position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragItem.instance.transform.position = eventData.position;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.instance.SetColor(0);
        }

        public void OnDrop(PointerEventData eventData)
        {
            eventData.pointerDrag.TryGetComponent<InventorySlotUI>(out var inventorySlot);
            if (inventorySlot)
            {
                DropIsInventorySlot(inventorySlot);
                return;
            }
            
            eventData.pointerDrag.TryGetComponent<QuickSlotUI>(out var quickSlot);
            if (quickSlot)
            {
                DropIsQuickSlot(quickSlot);
            }
        }

        private void DropIsInventorySlot(InventorySlotUI slot)
        {
            var inventory = Game.Instance.player.inventory;
            var droppedSlot = inventory.FindSlot(SlotType.Inventory, slot.GetSlotID());
            
            if (droppedSlot.GetSlotItemID() == "") return;

            switch (droppedSlot.GetItemType())
            {
                case ItemType.Use: 
                case ItemType.Inherent:
                case ItemType.Mineral:
                    var targetSlot = inventory.FindSlot(SlotType.Quick, quickSlotUI.slotID);
                    inventory.RegistrationQuickSlot(quickSlotUI.slotID, slot.GetSlotID());
                    quickSlotUI.SetSlot(targetSlot);
                    break;
            }
        }

        private void DropIsQuickSlot(QuickSlotUI slot)
        {
            var inventory = Game.instance.player.inventory;
            
            inventory.SwapQuickSlot(slot.slotID, quickSlotUI.slotID);
            
            var droppedChanged = inventory.FindSlot(SlotType.Quick, slot.slotID);
            slot.SetSlot(droppedChanged);
            
            var targetChanged= inventory.FindSlot(SlotType.Quick, quickSlotUI.slotID);
            quickSlotUI.SetSlot(targetChanged);
        }
    }
}
