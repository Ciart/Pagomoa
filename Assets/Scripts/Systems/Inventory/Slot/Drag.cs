using Ciart.Pagomoa.Entities.Players;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] public InventorySlot slot;
        [SerializeField] public InventorySlot item;
        public void OnBeginDrag(PointerEventData eventData)
        {
            var newPosition = new Vector3(eventData.position.x, eventData.position.y);
            var player = GameManager.instance.player;

            if (slot != null)
            {
                item = player.inventory.inventorySlots[slot.id];
                if(player.inventory.inventorySlots[slot.id].GetSlotItem() != null)
                    DragItem.instance.DragSetImage(player.inventory.inventorySlots[slot.id].GetSlotItem().sprite);
            }
            DragItem.instance.transform.position = newPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag.GetComponent<QuickSlotUI>())
                return;
            else
            {
                // ItemHoverObject.Instance.OffHover();
                DragItem.instance.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.instance.SetColor(0);
        }
    }
}
