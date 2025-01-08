using Ciart.Pagomoa.Entities.Players;
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
            PlayerController player = Game.instance.player;

            if (slot != null)
            {
                item = player.inventory.items[slot.id];
                if(player.inventory.items[slot.id].item != null )
                    DragItem.Instance.DragSetImage(player.inventory.items[slot.id].item.sprite);
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
