using System;
using Ciart.Pagomoa.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventoryDrag : MonoBehaviour
        , IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        private InventorySlotUI inventorySlotUI => GetComponent<InventorySlotUI>(); 
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            var inventory = GameManager.instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.GetSlotID());
            
            if (targetSlot == null) return;
            if (targetSlot.GetSlotItemID() == "") return;
            
            var hover = UIManager.instance.bookUI.GetHoverItemInfo();
            if (hover.isActiveAndEnabled) hover.gameObject.SetActive(false);
            
            var rightClickMenu = UIManager.instance.bookUI.GetRightClickMenu();
            rightClickMenu.DeleteMenu();
            rightClickMenu.transform.position = inventorySlotUI.transform.position;
            rightClickMenu.SetClickedSlot(inventorySlotUI);
            
            var itemType = targetSlot.GetItemType();
            
            switch (itemType)
            {
                case ItemType.Equipment:
                    rightClickMenu.EquipmentMenu();
                    break;    
                case ItemType.Mineral:
                    rightClickMenu.MineralMenu(targetSlot.GetSlotItemCount());
                    break;
                case ItemType.Use:
                    rightClickMenu.UseMenu();
                    break;
                case ItemType.Inherent:
                    rightClickMenu.InherentMenu();
                    break;
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.GetSlotID());
            
            if (targetSlot == null) return;
            if (targetSlot.GetSlotItemID() == "") return;
            
            DragItem.instance.DragSetImage(targetSlot.GetSlotItem().sprite);
            DragItem.instance.transform.position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData) { DragItem.instance.transform.position = eventData.position; }
        public void OnEndDrag(PointerEventData eventData) { DragItem.instance.SetColor(0); }
        
        public void OnDrop(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
            eventData.pointerPress.TryGetComponent<InventorySlotUI>(out var dragSlot);
            
            if (!dragSlot || inventorySlotUI.GetSlotID() == dragSlot.GetSlotID()) return;
            
            inventory.SwapInventorySlot(dragSlot.GetSlotID(), inventorySlotUI.GetSlotID());
            UIManager.instance.bookUI.GetInventoryUI().UpdateInventorySlot();
        }
    }
}
