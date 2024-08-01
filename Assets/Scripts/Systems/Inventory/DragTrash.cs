using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class DragTrash : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var data = eventData.pointerDrag.GetComponent<QuickSlotUI>();

            if (data)
            {
                GameManager.player.inventoryDB.SetQuickSlot(data.id, null);
            }
            else if (eventData.pointerPress.GetComponent<InventorySlotUI>())
            {
                GameManager.player.inventoryDB.RemoveItemData(eventData.pointerDrag.GetComponent<InventorySlotUI>().inventoryItem.item);
                InventoryUI.Instance.ResetSlot();
            }
        }
    }
}
