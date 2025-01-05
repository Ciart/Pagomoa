using Ciart.Pagomoa.Events;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlotDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        private QuickSlot slot => GetComponent<QuickSlot>();
        
        public void OnPointerDown(PointerEventData eventData)
        {
            var dragSlot = UIManager.instance.quickSlotUI.chosenSlot;
            if (dragSlot) return;
            
            UIManager.instance.quickSlotUI.chosenSlot = slot;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var dragSlot = UIManager.instance.quickSlotUI.chosenSlot;

            if (!dragSlot) return;
            
            DragItem.instance.DragSetImage(dragSlot.GetSlotItem().sprite);
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
        }

        public void OnDrop(PointerEventData eventData)
        {
            DragItem.instance.SetColor(0);
            UIManager.instance.bookUI.GetInventoryUI().chosenSlot = null;
            
            eventData.pointerDrag.TryGetComponent<InventorySlot>(out var inventorySlot);
            if (inventorySlot)
            {
                // eventData 가 InventorySlot 이면 QuickSlot 할당, player 것도 동기화
                ThatDropIsInventorySlot(inventorySlot);
                return;
            }
            
            eventData.pointerDrag.TryGetComponent<QuickSlot>(out var quickSlot);
            if (inventorySlot)
            {
                // eventData 가 QuickSlot 이면 swap   
                
            }
        }

        private void ThatDropIsInventorySlot(InventorySlot inventorySlot)
        {
            if (inventorySlot.GetSlotItem().id == "") return;
            
            slot.SetSlot(inventorySlot);
            slot.dependentID = inventorySlot.id;
            // 슬롯 바꿔도 괜찮은가?
            EventManager.Notify(new QuickSlotChangedEvent(slot.id, slot.dependentID));
        }

        private void ThatDropIsQuickSlot()
        {
            
        }
    }
}