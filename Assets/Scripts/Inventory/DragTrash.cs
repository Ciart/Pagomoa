using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTrash : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var Data = eventData.pointerDrag.GetComponent<QuickSlot>();

        if (Data)
        {
            QuickSlotItemDB.instance.quickSlotItems.RemoveAt(Data.id);
            QuickSlotItemDB.instance.quickSlotItems.Insert(Data.id, null);
            Data.inventoryItem = null;
            Data.itemImage.sprite = Data.transparentImage;
            Data.itemCount.text = "";
        }
        else if (eventData.pointerPress.GetComponent<Slot>())
        {
            InventoryDB.Instance.DeleteItem(eventData.pointerDrag.GetComponent<Slot>().inventoryItem.item);
            EtcInventory.Instance.ResetSlot();
        }
    }
}
