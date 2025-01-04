using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            var dragSlot = UIManager.instance.bookUI.GetInventoryUI().chosenSlot;
            
            if (dragSlot != null)
            {
                if(dragSlot.GetSlotItem().id != string.Empty)
                    DragItem.instance.DragSetImage(dragSlot.GetSlotItem().sprite);
                
                DragItem.instance.transform.position = eventData.position;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag.GetComponent<QuickSlotUI>())
            {
                
            }
            else
            {
                // ItemHoverObject.Instance.OffHover();
                DragItem.instance.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.instance.SetColor(0);
            UIManager.instance.bookUI.GetInventoryUI().chosenSlot = null;
        }
    }
}
