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
            var inventory = GameManager.instance.player.inventory;
            
            if (data)
            {
                inventory.SetQuickItem(data.id, null);
            }
            else if (eventData.pointerPress.GetComponent<InventorySlot>())
            {
                inventory.RemoveItemData(inventory.inventorySlots[
                    eventData.pointerDrag.GetComponent<InventorySlot>().id].GetSlotItem());
                UIManager.instance.bookUI.GetInventoryUI().UpdateSlots();
            }
        }
    }
}
