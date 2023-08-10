using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Player;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image oxygenbar;
    [SerializeField] Image hungrybar;
    [SerializeField] Image digbar;
    public GameObject InventoryUI;
    bool ActiveInventory = false;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerDigger>().DiggingEvent.AddListener(SetDigGage);
        player.GetComponent<Status>().oxygenAlter.AddListener(UpdateOxygenBar);
        player.GetComponent<Status>().hungryAlter.AddListener(UpdateHungryBar);
        InventoryUI.SetActive(ActiveInventory);
    }
    private void Update()
    {
        SetInventoryUI();
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
        digbar.enabled = false;
    }
    public void SetDigGage(float holdtime, float digtime)
    {
        digbar.fillAmount = holdtime / digtime;
        digbar.enabled = true;
    }
    private void SetInventoryUI()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ActiveInventory = !ActiveInventory;
            InventoryUI.SetActive(ActiveInventory);
        }
    }
    private void ControlQuickSlot()
    {
        int i = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (; i < QuickSlotItemDB.instance.quickSlots.Count; i++)
            {
                if (i == 0)
                {
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(true);
                    QuickSlotItemDB.instance.selectedSlot = QuickSlotItemDB.instance.quickSlots[i];
                }
                else
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (; i < QuickSlotItemDB.instance.quickSlots.Count; i++)
            {
                if (i == 1)
                {
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(true);
                    QuickSlotItemDB.instance.selectedSlot = QuickSlotItemDB.instance.quickSlots[i];
                }
                else
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (; i < QuickSlotItemDB.instance.quickSlots.Count; i++)
            {
                if (i == 2)
                {
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(true);
                    QuickSlotItemDB.instance.selectedSlot = QuickSlotItemDB.instance.quickSlots[i];
                }
                else
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            for (; i < QuickSlotItemDB.instance.quickSlots.Count; i++)
            {
                if (i == 3)
                {
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(true);
                    QuickSlotItemDB.instance.selectedSlot = QuickSlotItemDB.instance.quickSlots[i];
                }
                else
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            for (; i < QuickSlotItemDB.instance.quickSlots.Count; i++)
            {
                if (i == 4)
                {
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(true);
                    QuickSlotItemDB.instance.selectedSlot = QuickSlotItemDB.instance.quickSlots[i];
                }
                else
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            for (; i < QuickSlotItemDB.instance.quickSlots.Count; i++)
            {
                if (i == 5)
                {
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(true);
                    QuickSlotItemDB.instance.selectedSlot = QuickSlotItemDB.instance.quickSlots[i];
                }
                else
                    QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(false);
            }
        }
        else
            return;
    }
    private void UseQuickSlot()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (QuickSlotItemDB.instance.selectedSlot.inventoryItem.item.itemType == Itembefore.ItemType.Use)
            {
                QuickSlotItemDB.instance.selectedSlot.inventoryItem.count -= 1;
                QuickSlotItemDB.instance.selectedSlot.SetItemCount();

                if (QuickSlotItemDB.instance.selectedSlot.inventoryItem.count == 0)
                {
                    InventoryDB.Instance.items.Remove(QuickSlotItemDB.instance.selectedSlot.inventoryItem);
                    QuickSlotItemDB.instance.selectedSlot.SetSlotNull();
                }
                QuickSlotItemDB.instance.selectedSlot.inventory.UpdateSlot();
            }
            else
                return;
        }
    }
}

