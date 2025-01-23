using Ciart.Pagomoa.Events;
using UnityEditor.Experimental.GraphView;
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
            var inventory = GameManager.instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Quick, quickSlotUI.slotID);

            if (targetSlot == null) return;
            
            UIManager.instance.quickUI.SelectQuickSlot(quickSlotUI.transform.GetSiblingIndex());;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
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
                // eventData 가 InventorySlot 이면 QuickSlot 할당, player 것도 동기화
                DropIsInventorySlot(inventorySlot);
                return;
            }
            
            eventData.pointerDrag.TryGetComponent<QuickSlotUI>(out var quickSlot);
            if (quickSlot)
            {
                // eventData 가 QuickSlot 이면 swap 
                DropIsQuickSlot(quickSlot);
            }
        }

        private void DropIsInventorySlot(InventorySlotUI slot)
        {
            var inventory = GameManager.instance.player.inventory;
            var droppedSlot = inventory.FindSlot(SlotType.Inventory, slot.slotID);
            
            if (droppedSlot == null) return;
            if (droppedSlot.GetSlotItemID() == "") return;
            
            var targetSlot = inventory.FindSlot(SlotType.Quick, quickSlotUI.slotID);
            if (targetSlot == null) return;
            inventory.RegistrationQuickSlot(quickSlotUI.slotID, slot.slotID);
            
            var updatedSlot = inventory.FindSlot(SlotType.Quick, slot.slotID);
            if (updatedSlot == null) return;
            quickSlotUI.SetSlot(updatedSlot);
            
            UIManager.instance.quickUI.UpdateQuickSlot();
        }

        private void DropIsQuickSlot(QuickSlotUI slot)
        {
            var inventory = GameManager.instance.player.inventory;
            var droppedSlot = inventory.FindSlot(SlotType.Quick, slot.slotID);
            
            if (droppedSlot == null) return;
            
            inventory.SwapQuickSlot(slot.slotID, quickSlotUI.slotID);
            
            var droppedChanged = inventory.FindSlot(SlotType.Quick, slot.slotID);
            if (droppedChanged == null) return;
            slot.SetSlot(droppedChanged);
            
            var targetChanged= inventory.FindSlot(SlotType.Quick, quickSlotUI.slotID);
            if (targetChanged == null) return;
            quickSlotUI.SetSlot(targetChanged);
            
            UIManager.instance.quickUI.UpdateQuickSlot();
        }
    }
}