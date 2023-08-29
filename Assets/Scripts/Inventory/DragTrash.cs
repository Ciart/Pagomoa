using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTrash : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerPress.GetComponent<QuickSlot>())
        {
            QuickSlotItemDB.instance.quickSlotItems.RemoveAt(eventData.pointerPress.GetComponent<QuickSlot>().id);
            QuickSlotItemDB.instance.quickSlotItems.Insert(eventData.pointerPress.GetComponent<QuickSlot>().id, null);
            eventData.pointerPress.GetComponent<QuickSlot>().inventoryItem = null;
            eventData.pointerPress.GetComponent<QuickSlot>().itemImage.sprite = eventData.pointerPress.GetComponent<QuickSlot>().transparentImage;
            eventData.pointerPress.GetComponent<QuickSlot>().itemCount.text = "";
        }
        else if (eventData.pointerPress.GetComponent<Slot>())
        {
            InventoryDB.Instance.items.Remove(eventData.pointerPress.GetComponent<Slot>().inventoryItem);
            EtcInventory.Instance.ResetSlot();

        }
    }
}
