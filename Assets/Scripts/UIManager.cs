using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Player;
using Unity.VisualScripting;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image oxygenbar;
    [SerializeField] Image hungrybar;
    [SerializeField] Image digbar;
    [SerializeField] public GameObject InventoryUI;
    [SerializeField] public GameObject EscUI;

    private PlayerInput playerInput;

    bool ActiveInventory = false;
    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerDigger>().DiggingEvent.AddListener(SetDigGage);
        player.GetComponent<PlayerDigger>().digEndEvent.AddListener(SetDigGagefalse);
        player.GetComponent<Status>().oxygenAlter.AddListener(UpdateOxygenBar);
        player.GetComponent<Status>().hungryAlter.AddListener(UpdateHungryBar);
        InventoryUI.SetActive(ActiveInventory);
        SetDigGagefalse();

        playerInput = player.GetComponent<PlayerInput>();

        playerInput.Actions.Slot1.started += context => {ControlQuickSlot(0);};
        playerInput.Actions.Slot2.started += context => {ControlQuickSlot(1);};
        playerInput.Actions.Slot3.started += context => {ControlQuickSlot(2);};
        playerInput.Actions.Slot4.started += context => {ControlQuickSlot(3);};
        playerInput.Actions.Slot5.started += context => {ControlQuickSlot(4);};
        playerInput.Actions.Slot6.started += context => {ControlQuickSlot(5);};

        playerInput.Actions.SetEscUI.started += context => 
        {
            bool activeEscUI = false;
            if (EscUI.activeSelf == false)
                activeEscUI = !activeEscUI;
            EscUI.SetActive(activeEscUI);
        };

        playerInput.Actions.SetInventoryUI.started += context =>
        {
            ActiveInventory = !ActiveInventory;
            InventoryUI.SetActive(ActiveInventory);
            if (InventoryUI.activeSelf == false)
                HoverEvent.Instance.image.SetActive(ActiveInventory);
        };
        playerInput.Actions.UseQuickSlot.started += context =>
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
                EtcInventory.Instance.UpdateSlot();
            }
            else if (QuickSlotItemDB.instance.selectedSlot.inventoryItem.item.itemType == Item.ItemType.Inherent)
            {
                QuickSlotItemDB.instance.selectedSlot.UseItem();
                
                QuickSlotItemDB.instance.selectedSlot.inventory.UpdateSlot();
            } else
                return;
        };
    }
    public void UpdateOxygenBar(float current_oxygen, float max_oxygen)
    {
        oxygenbar.fillAmount = current_oxygen / max_oxygen;
    }

    public void UpdateHungryBar(float current_hungry, float max_hungry)
    {
        hungrybar.fillAmount = current_hungry / max_hungry;
    }
    public void SetDigGagefalse()
    {
        digbar.transform.parent.gameObject.SetActive(false);
    }
    public void SetDigGage(float holdtime, float digtime)
    {
        digbar.fillAmount = holdtime / digtime;
        digbar.transform.parent.gameObject.SetActive(true);
    }
    private void ControlQuickSlot(int n)
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

