using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTrash : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<QuickSlot>())
        {
            QuickSlotItemDB.instance.quickSlotItems.RemoveAt(eventData.pointerDrag.GetComponent<QuickSlot>().id);
            QuickSlotItemDB.instance.quickSlotItems.Insert(eventData.pointerDrag.GetComponent<QuickSlot>().id, null);
            eventData.pointerDrag.GetComponent<QuickSlot>().inventoryItem = null;
            eventData.pointerDrag.GetComponent<QuickSlot>().itemImage.sprite = eventData.pointerDrag.GetComponent<QuickSlot>().transparentImage;
            eventData.pointerDrag.GetComponent<QuickSlot>().itemCount.text = "";
        }
        else if (eventData.pointerPress.GetComponent<Slot>())
        {
            InventoryDB.Instance.items.Remove(eventData.pointerDrag.GetComponent<Slot>().inventoryItem);
            EtcInventory.Instance.ResetSlot();

        }
    }
}
