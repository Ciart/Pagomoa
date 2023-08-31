using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Player;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image oxygenbar;
    [SerializeField] Image hungrybar;
    [SerializeField] Image digbar;
    [SerializeField] public GameObject InventoryUI;
    [SerializeField] public GameObject EscUI;

    bool ActiveInventory = false;
    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerDigger>().DiggingEvent.AddListener(SetDigGage);
        player.GetComponent<PlayerDigger>().digEndEvent.AddListener(SetDigGagefalse);
        player.GetComponent<Status>().oxygenAlter.AddListener(UpdateOxygenBar);
        player.GetComponent<Status>().hungryAlter.AddListener(UpdateHungryBar);
        InventoryUI.SetActive(ActiveInventory);
    }
    private void Update()
    {
        SetInventoryUI();
        SetEscUI();
        ControlQuickSlot();
        UseQuickSlot();
    }
    public void UpdateOxygenBar(float current_oxygen, float max_oxygen)
    {
        oxygenbar.fillAmount = current_oxygen / max_oxygen;
    }

    public void UpdateHungryBar(float current_hungry, float max_hungry)
    {
        hungrybar.fillAmount = current_hungry / max_hungry;
    }
    // public void SetPlayerUIDirection(float direction)
    // {
    //     transform.localScale = new Vector3(direction, 1, 1);
    // }
    public void SetDigGagefalse()
    {
        digbar.transform.parent.gameObject.SetActive(false);
    }
    public void SetDigGage(float holdtime, float digtime)
    {
        digbar.fillAmount = holdtime / digtime;
        digbar.transform.parent.gameObject.SetActive(true);
    }
    private void SetInventoryUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ActiveInventory = !ActiveInventory;
            InventoryUI.SetActive(ActiveInventory);
            if(InventoryUI.activeSelf == false)
                HoverEvent.Instance.image.SetActive(ActiveInventory);
        }
    }
    public void SetEscUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool activeEscUI = false;
            if (EscUI.activeSelf == false)
                activeEscUI = !activeEscUI;
            EscUI.SetActive(activeEscUI);
        }
    }
    private void ControlQuickSlot()
    {
        int i = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            i = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            i = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            i = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            i = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            i = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            i = 5;
        if (i == -1) return;

        for (int j = 0; j < QuickSlotItemDB.instance.quickSlots.Count; j++)
        {
            if (i == j)
            {
                QuickSlotItemDB.instance.quickSlots[j].selectedSlotImage.gameObject.SetActive(true);
                //QuickSlotItemDB.instance.quickSlots[j].transform.SetAsLastSibling();
                if (QuickSlotItemDB.instance.selectedSlot != QuickSlotItemDB.instance.quickSlots[j])
                {
                    QuickSlotItemDB.instance.selectedSlot = QuickSlotItemDB.instance.quickSlots[j];
                }
                else
                {
                    QuickSlotItemDB.instance.quickSlots[j].selectedSlotImage.gameObject.SetActive(false);
                    QuickSlotItemDB.instance.selectedSlot = null;
                }
            }
            else
                QuickSlotItemDB.instance.quickSlots[j].selectedSlotImage.gameObject.SetActive(false);
        }
    }
    private void UseQuickSlot()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
                    QuickSlotItemDB.instance.selectedSlot.itemImage.sprite = QuickSlot.Instance.transparentImage;
                }
                QuickSlotItemDB.instance.selectedSlot.inventory.UpdateSlot();
            }
            else if (QuickSlotItemDB.instance.selectedSlot.inventoryItem.item.itemType == Item.ItemType.Inherent)
            {
                QuickSlotItemDB.instance.selectedSlot.UseItem();
                
                QuickSlotItemDB.instance.selectedSlot.inventory.UpdateSlot();
            } else
                return;
        }
    }
}

