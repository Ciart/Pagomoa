using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public InventoryDB inventoryDB;
    public ShopDB shopDB;
    public GameObject slots;
    public GameObject slot;
    public GameObject Equipmentslots;
    public GameObject Equipmentslot;
    public GameObject consumptionSlots;
    public GameObject consumptionSlot;
    private void DeleteEtcSlot()
    {
        for(int i = 0; i < slots.transform.childCount; i++)
        {
            GameObject slot = slots.transform.GetChild(i).gameObject;
            if (slot.activeSelf)
                Destroy(slot);
        }
    }
    private void DeleteConsumptionSlot()
    {
        for (int i = 0; i < consumptionSlots.transform.childCount; i++)
        {
            GameObject slot = consumptionSlots.transform.GetChild(i).gameObject;
            if (slot.activeSelf)
                Destroy(slot);
        }
    }
    private void DeleteArtifactSlot()
    {
        for (int i = 0; i < Equipmentslots.transform.childCount; i++)
        {
            GameObject slot = Equipmentslots.transform.GetChild(i).gameObject;
            if (slot.activeSelf)
                Destroy(slot);
        }
    }
    private void AddEtcSlot(MineralData data)
    {
        GameObject SpawnedSlot = Instantiate(slot, slots.transform);
        SpawnedSlot.SetActive(true);
        SpawnedSlot.GetComponent<Slot>().SetEtcSlot(data, inventoryDB.MineralsData[data]);
    }
    private void AddEquipmentSlot(ShopItemData data)
    {
        GameObject SpawnedSlot = Instantiate(Equipmentslot, Equipmentslots.transform);
        SpawnedSlot.SetActive(true);
        SpawnedSlot.GetComponent<Slot>().SetEquipmentSlotShop(data);
    }
    public void AddConsumptionSlot(ShopConsumptionItemData data)
    {
        GameObject SpawnedSlot = Instantiate(consumptionSlot, consumptionSlots.transform);
        SpawnedSlot.SetActive(true);
        SpawnedSlot.GetComponent<Slot>().SetConsumptionslot(data, inventoryDB.ConsumptionItemsData[data]);
    }
    public void UpdateEtcSlot()
    {
        DeleteEtcSlot();
        foreach(MineralData key in inventoryDB.MineralsData.Keys)
            AddEtcSlot(key);
    }
    public void UpdateEquipmentSlot()
    {
        DeleteArtifactSlot();
        foreach (ShopItemData key in inventoryDB.ShopItemsData)
            AddEquipmentSlot(key);
    }
    public void UpdateConsumptionSlot()
    {
        DeleteConsumptionSlot();
        foreach (ShopConsumptionItemData key in inventoryDB.ConsumptionItemsData.Keys)
        {
            AddConsumptionSlot(key);
        }
    }
}
