using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            var slot = eventData.pointerEnter.GetComponent<InventorySlotUI>();
            if (slot.inventoryItem.item == null)
                return;
            
            Vector3 newPosition = new Vector3(eventData.position.x + 5, eventData.position.y);
            // InventoryUIManager.Instance.ItemHoverObject.SetActive(true);
            // InventoryUIManager.Instance.ItemHoverObject.transform.position = newPosition;
            // InventoryUIManager.Instance.ItemHoverObject.GetComponent<ItemHoverObject>().WriteText(slot);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            // InventoryUIManager.Instance.ItemHoverObject.SetActive(false);
        }
    }
}
