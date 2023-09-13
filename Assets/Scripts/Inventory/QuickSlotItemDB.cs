using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuickSlotItemDB : MonoBehaviour
{
   
    static public QuickSlotItemDB instance;

    [SerializeField] public List<InventoryItem> quickSlotItems = new List<InventoryItem>();
    [SerializeField] public List<QuickSlot> quickSlots = new List<QuickSlot>();
    [SerializeField] public QuickSlot selectedSlot;

    private void Start()
    {
        instance = this;
    }
    public void UseQuickSlot()
    {
        if (selectedSlot == null || selectedSlot.inventoryItem.item == null)
            return;

        if (selectedSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            selectedSlot.inventoryItem.count -= 1;
            selectedSlot.SetItemCount();
            selectedSlot.UseItem();

            if (selectedSlot.inventoryItem.count == 0)
            {
                InventoryDB.Instance.items.Remove(selectedSlot.inventoryItem);
                selectedSlot.SetSlotNull();
            }
            EtcInventory.Instance.UpdateSlot();
        }
        else if (selectedSlot.inventoryItem.item.itemType == Item.ItemType.Inherent)
        {
            selectedSlot.UseItem();

            selectedSlot.inventory.UpdateSlot();
        }
        else
            return;
    }
    public void ControlQuickSlot(int n)
    {
        for (int index = 0; index < quickSlots.Count; index++)
        {
            if (n == index)
            {
                if (selectedSlot != quickSlots[index])
                {
                    selectedSlot = quickSlots[index];
                    quickSlots[index].selectedSlotImage.gameObject.SetActive(true);
                    selectedSlot.transform.SetAsLastSibling();
                }
                else
                {
                    quickSlots[index].selectedSlotImage.gameObject.SetActive(false);
                    selectedSlot = null;
                }
            }
            else
                quickSlots[index].selectedSlotImage.gameObject.SetActive(false);
        }

    }
}
