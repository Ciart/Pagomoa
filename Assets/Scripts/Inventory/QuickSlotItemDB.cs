using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
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
    private void UseQuickSlot()
    {
        if (QuickSlotItemDB.instance.selectedSlot == null || QuickSlotItemDB.instance.selectedSlot.inventoryItem.item == null)
            return;

        if (QuickSlotItemDB.instance.selectedSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            QuickSlotItemDB.instance.selectedSlot.inventoryItem.count -= 1;
            QuickSlotItemDB.instance.selectedSlot.SetItemCount();
            QuickSlotItemDB.instance.selectedSlot.UseItem();
            if (QuickSlotItemDB.instance.selectedSlot.inventoryItem.count == 0)
            {
                InventoryDB.Instance.items.Remove(QuickSlotItemDB.instance.selectedSlot.inventoryItem);
                QuickSlotItemDB.instance.selectedSlot.SetSlotNull();
            }
            EtcInventory.Instance.DeleteSlot();
            EtcInventory.Instance.UpdateSlot();
        }
        else if (QuickSlotItemDB.instance.selectedSlot.inventoryItem.item.itemType == Item.ItemType.Inherent)
        {
            QuickSlotItemDB.instance.selectedSlot.UseItem();
        }
        else
            return;
    }
    public void ControlQuickSlot(int n)
    {
        for (int index = 0; index < QuickSlotItemDB.instance.quickSlots.Count; index++)
        {
            if (n == index)
            {
                if (QuickSlotItemDB.instance.selectedSlot != QuickSlotItemDB.instance.quickSlots[index])
                {
                    QuickSlotItemDB.instance.selectedSlot = QuickSlotItemDB.instance.quickSlots[index];
                    QuickSlotItemDB.instance.quickSlots[index].selectedSlotImage.gameObject.SetActive(true);
                    QuickSlotItemDB.instance.selectedSlot.transform.SetAsLastSibling();
                }
                else
                {
                    QuickSlotItemDB.instance.quickSlots[index].selectedSlotImage.gameObject.SetActive(false);
                    QuickSlotItemDB.instance.selectedSlot = null;
                }
            }
            else
                QuickSlotItemDB.instance.quickSlots[index].selectedSlotImage.gameObject.SetActive(false);
        }

    }
}
