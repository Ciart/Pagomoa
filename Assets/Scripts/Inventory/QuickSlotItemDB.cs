using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Unity.VisualScripting;

public class QuickSlotItemDB : MonoBehaviour
{
   
    static public QuickSlotItemDB instance;

    [SerializeField] public List<InventoryItem> quickSlotItems = new List<InventoryItem>();
    [SerializeField] public List<QuickSlot> quickSlots = new List<QuickSlot>();
    [SerializeField] public QuickSlot selectedSlot;

    private PlayerInput playerInput;

    private void Start()
    {
        instance = this;
    }
    private void Awake()
    {

        GameObject player = GameObject.Find("Player");

        playerInput = player.GetComponent<PlayerInput>();

        playerInput.Actions.Slot1.started += context => { ControlQuickSlot(0); };
        playerInput.Actions.Slot2.started += context => { ControlQuickSlot(1); };
        playerInput.Actions.Slot3.started += context => { ControlQuickSlot(2); };
        playerInput.Actions.Slot4.started += context => { ControlQuickSlot(3); };
        playerInput.Actions.Slot5.started += context => { ControlQuickSlot(4); };
        playerInput.Actions.Slot6.started += context => { ControlQuickSlot(5); };

        playerInput.Actions.UseQuickSlot.started += context =>
        {
            UseQuickSlot();
        };
    }
    private void Remove(Item data)
    {
        var quickSlotItem = quickSlotItems.Find(quickSlotItem => quickSlotItem.item == data);

        for(int i = 0; i < quickSlotItems.Count; i++)
        {
            if (quickSlotItems[i].item == quickSlotItem.item)
            {
                quickSlotItems.RemoveAt(i);
                quickSlotItems.Insert(i, new InventoryItem(null, 0));
            }
        }
    }
    private void UseQuickSlot()
    {
        if (selectedSlot == null || selectedSlot.inventoryItem == null || selectedSlot.inventoryItem.item == null)
            return;

        if (selectedSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            selectedSlot.inventoryItem.count -= 1;
            selectedSlot.SetItemCount();
            selectedSlot.UseItem();
            if (selectedSlot.inventoryItem.count == 0)
            {
                InventoryDB.Instance.Remove(selectedSlot.inventoryItem.item);
                Remove(selectedSlot.inventoryItem.item);
                selectedSlot.SetSlotNull();
            }
            EtcInventory.Instance.DeleteSlot();
            EtcInventory.Instance.UpdateSlot();
        }
        else if (selectedSlot.inventoryItem.item.itemType == Item.ItemType.Inherent)
        {
            selectedSlot.UseItem();
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
