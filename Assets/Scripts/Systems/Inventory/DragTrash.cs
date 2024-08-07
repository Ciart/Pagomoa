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
                GameManager.player.inventory.SetQuickItem(data.id, null);
            }
            else if (eventData.pointerPress.GetComponent<InventorySlotUI>())
            {
                GameManager.player.inventory.RemoveItemData(GameManager.player.inventory.items[
                    eventData.pointerDrag.GetComponent<InventorySlotUI>().id].item);
                InventoryUI.Instance.UpdateSlots();
            }
        }
    }
}
