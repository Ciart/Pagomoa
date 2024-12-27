using Ciart.Pagomoa.Entities.Players;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class DragTrash : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var data = eventData.pointerDrag.GetComponent<QuickSlotUI>();
            PlayerController player = GameManager.instance.player;

            if (data)
            {
                player.inventory.SetQuickItem(data.id, null);
            }
            else if (eventData.pointerPress.GetComponent<InventorySlotUI>())
            {
                player.inventory.RemoveItemData(player.inventory.items[
                    eventData.pointerDrag.GetComponent<InventorySlotUI>().id].item);
                UIManager.instance.bookUI.inventoryUI.UpdateSlots();
            }
        }
    }
}
