using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public InventoryDB inventoryDB;
    public GameObject slots;
    public GameObject slot;
    private void DeleteSlot()
    {
        for(int i = 0; i < slots.transform.childCount; i++)
        {
            GameObject slot = slots.transform.GetChild(i).gameObject;
            if (slot.activeSelf)
                Destroy(slot);
        }
    }
    private void AddSlot(MineralData data)
    {
        GameObject SpawnedSlot = Instantiate(slot, slots.transform);
        SpawnedSlot.SetActive(true);
        SpawnedSlot.GetComponent<Slot>().SetSlot(data, inventoryDB.MineralsData[data]);
    }
    public void UpdateSlot()
    {
        DeleteSlot();
        foreach(MineralData key in inventoryDB.MineralsData.Keys)
            AddSlot(key);
    }
}
