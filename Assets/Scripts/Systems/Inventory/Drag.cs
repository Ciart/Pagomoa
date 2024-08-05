using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] public InventorySlotUI slot;
        [SerializeField] public InventorySlot item;
        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 newPosition = new Vector3(eventData.position.x, eventData.position.y);

            if (slot != null)
            {
                item = slot.slot;
                if(slot.slot.item != null )
                    DragItem.Instance.DragSetImage(slot.slot.item.itemImage);
            }
            DragItem.Instance.transform.position = newPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag.GetComponent<QuickSlotUI>())
                return;
            else
            {
                // ItemHoverObject.Instance.OffHover();
                DragItem.Instance.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.Instance.SetColor(0);
        }
    }
}
