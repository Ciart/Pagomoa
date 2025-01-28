using Ciart.Pagomoa.Events;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlotDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        private QuickSlot quickSlot => GetComponent<QuickSlot>();
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (quickSlot.slot.GetSlotItemID() == "") return;
            
            UIManager.instance.quickSlotUI.SelectQuickSlot(quickSlot.transform.GetSiblingIndex());;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var dragSlot = UIManager.instance.quickSlotUI.chosenSlot;

            if (!dragSlot) return;
            
            DragItem.instance.DragSetImage(dragSlot.slot.GetSlotItem().sprite);
            DragItem.instance.transform.position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragItem.instance.transform.position = eventData.position;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.instance.SetColor(0);
            UIManager.instance.quickSlotUI.chosenSlot = null;
            
            if (eventData.pointerCurrentRaycast.gameObject.layer != LayerMask.NameToLayer("UI"))
                quickSlot.ResetSlot();
        }

        public void OnDrop(PointerEventData eventData)
        {
            DragItem.instance.SetColor(0);
            UIManager.instance.GetUIContainer().SetChosenSlot(null);
            
            eventData.pointerDrag.TryGetComponent<InventorySlot>(out var inventorySlot);
            if (inventorySlot)
            {
                // eventData 가 InventorySlot 이면 QuickSlot 할당, player 것도 동기화
                ThatDropIsInventorySlot(inventorySlot);
                return;
            }
            
            eventData.pointerDrag.TryGetComponent<QuickSlot>(out var quickSlot);
            if (!quickSlot) return;
            if (quickSlot.slot.GetSlotItemID() == "") return;
            
            // eventData 가 QuickSlot 이면 swap 
            ThatDropIsQuickSlot(quickSlot);
        }

        private void ThatDropIsInventorySlot(InventorySlot droppedInventorySlot)
        {
            if (droppedInventorySlot.slot.GetSlotItemID() == "") return;
            
            var inventory = Game.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            
            if (inventory.quickItems[quickSlot.id].GetSlotItemID() != "")
                inventoryUI.SwapReferenceSlotID(-1, quickSlot.id, true);
            
            inventory.quickItems[quickSlot.id].SetSlotItemID(droppedInventorySlot.slot.GetSlotItemID());
            inventory.quickItems[quickSlot.id].SetSlotItemCount(droppedInventorySlot.slot.GetSlotItemCount());
            
            quickSlot.SetSlot(droppedInventorySlot.slot);
            droppedInventorySlot.referenceSlotID = quickSlot.id;
            UIManager.instance.quickSlotUI.UpdateQuickSlot();
        }

        private void ThatDropIsQuickSlot(QuickSlot droppedQuickSlot)
        {
            var inventory = Game.instance.player.inventory;
            
            if (quickSlot.slot.GetSlotItemID() == "")
            {
                quickSlot.SetSlot(droppedQuickSlot.slot);
                droppedQuickSlot.ResetSlot();
                
                UIManager.instance.bookUI.GetInventoryUI().
                    SwapReferenceSlotID(quickSlot.id, droppedQuickSlot.id, true);
            }
            else if (quickSlot.slot.GetSlotItemID() != "")
            {
                var tempSlot = new Slot();
                tempSlot.SetSlotItemID(quickSlot.slot.GetSlotItemID());
                tempSlot.SetSlotItemCount(quickSlot.slot.GetSlotItemCount());
                
                quickSlot.SetSlot(droppedQuickSlot.slot);
                droppedQuickSlot.SetSlot(tempSlot);
                
                UIManager.instance.bookUI.GetInventoryUI().
                    SwapReferenceSlotID(quickSlot.id, droppedQuickSlot.id);
            }
            
            (inventory.quickItems[quickSlot.id], inventory.quickItems[droppedQuickSlot.id])
                = (inventory.quickItems[droppedQuickSlot.id], inventory.quickItems[quickSlot.id]);
        }
    }
}
