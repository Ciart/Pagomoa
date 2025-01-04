using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
    {
        private InventorySlot slot => GetComponent<InventorySlot>(); 
        
        public void OnPointerDown(PointerEventData eventData)
        {
            var dragSlot = UIManager.instance.bookUI.GetInventoryUI().chosenSlot;
            
            if (dragSlot) return;
            
            UIManager.instance.bookUI.GetInventoryUI().chosenSlot = slot;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            var dragSlot = UIManager.instance.bookUI.GetInventoryUI().chosenSlot;

            if (!dragSlot) return;
            
            DragItem.instance.DragSetImage(dragSlot.GetSlotItem().sprite);
            DragItem.instance.transform.position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            /*if (eventData.pointerDrag.GetComponent<QuickSlotUI>())
            {
                
            }
            else
            {
                // ItemHoverObject.Instance.OffHover();
                
            }*/
            
            DragItem.instance.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.instance.SetColor(0);
            UIManager.instance.bookUI.GetInventoryUI().chosenSlot = null;
        }
    }
}
