using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
    {
        private InventorySlot inventorySlot => GetComponent<InventorySlot>(); 
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (inventorySlot.slot.GetSlotItemID() == "") return;
            
            UIManager.instance.GetUIContainer().SetChosenSlot(inventorySlot);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
            var dragSlot = UIManager.instance.GetUIContainer().chosenSlot;
            
            if (dragSlot.IsUnityNull()) return;
            
            var targetSlot = inventory.FindInventorySlotByID(dragSlot); 
            
            if (targetSlot.GetSlotItemID() == "")
            {
                UIManager.instance.GetUIContainer().SetChosenSlot(null);
                return;
            }
            
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
            UIManager.instance.GetUIContainer().SetChosenSlot(null);
        }
    }
}
