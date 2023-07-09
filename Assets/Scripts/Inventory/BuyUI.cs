using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BuyUI : MonoBehaviour
{
    public ShopDB shopDB;
    public GameObject slot;
    public GameObject slots;
    public GameObject consumptionSlot;
    public GameObject consumptionSlots;
    private void DeleteEquipmentSlotShop()
    {
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            GameObject slot = slots.transform.GetChild(i).gameObject;
            if (slot.activeSelf)
                Destroy(slot);
        }
    }
    private void DeleteConsumptionSlotShop()
    {
        for (int i = 0; i < consumptionSlots.transform.childCount; i++)
        {
            GameObject slot = consumptionSlots.transform.GetChild(i).gameObject;
            if (slot.activeSelf)
                Destroy(slot);
        }
    }
    private void AddEquipmentSlotShop(Item data)
    {
        GameObject SpawnedSlot = Instantiate(slot, slots.transform);
        SpawnedSlot.SetActive(true);
        //SpawnedSlot.GetComponent<Slot>().SetEquipmentSlotShop(data);
    }
    private void AddConsumptionSlotShop(Item data)
    {
        GameObject SpawnedSlot = Instantiate(consumptionSlot, consumptionSlots.transform);
        SpawnedSlot.SetActive(true);
        //SpawnedSlot.GetComponent<Slot>().SetConsumptionSlotShop(data);
    }
    public void UpdateEquipmentSlotShop()
    {
        DeleteEquipmentSlotShop();
        foreach (Item key in shopDB.items)
            AddEquipmentSlotShop(key);
    }
    public void UpdateConsumptionSlotShop()
    {
        DeleteConsumptionSlotShop();
        foreach (Item key in shopDB.consumptionitems)
            AddConsumptionSlotShop(key);
    }
    private void Awake()
    {
        UpdateEquipmentSlotShop();
        UpdateConsumptionSlotShop();
    }
}
