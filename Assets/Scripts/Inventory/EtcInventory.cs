using Maps;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class EtcInventory : MonoBehaviour
{
    [SerializeField] private InventoryDB inventoryDB;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject slot;
    public int slotCount;

    private void Awake()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject SpawnedSlot = Instantiate(slot, slotParent.transform);
            SpawnedSlot.SetActive(true);
        }
    }
    public void InputSlot(Mineral data, int id)
    {
        Slot[] slotDatas = slotParent.GetComponentsInChildren<Slot>();
        foreach (Slot slot in slotDatas)
        {
            if (slot.id == id)
                slot.SetItem(data, inventoryDB.MineralsData[data]);
        }
    }
    public void NotNull(Mineral data)
    {
        Slot[] slotDatas = slotParent.GetComponentsInChildren<Slot>();
        foreach (Slot slot in slotDatas)
        {
            if (slot.mineralItem == data.item && slot.mineralItem.itemName == "CopperItem")
            {
                Debug.Log("널 아닌 쿠퍼");
                slot.SetItem(data, inventoryDB.mineralCount.copperCount.Count);
                break;
            }
            else if (slot.mineralItem == data.item && slot.mineralItem.itemName == "IronItem")
            {
                Debug.Log("널 아닌 아이언");
                slot.SetItem(data, inventoryDB.mineralCount.ironCount.Count);
                break;
            }
        }
    }
    public void InputSlot(Mineral data)
    {
        Slot[] slotDatas = slotParent.GetComponentsInChildren<Slot>();
        foreach (Slot slot in slotDatas)
        {
            if (slot.mineralItem == null && data.item.itemName == "CopperItem")
            {
                Debug.Log("널인 쿠퍼");
                slot.SetItem(data, inventoryDB.mineralCount.copperCount.Count);
                break;
            }
            else if (slot.mineralItem == null && data.item.itemName == "IronItem")
            {
                Debug.Log("널인 아이언");
                slot.SetItem(data, inventoryDB.mineralCount.ironCount.Count);
                break;
            }
        }
        //bool isAssigned = false;
        //foreach (Slot slot in slotDatas)
        //{
        //    if (slot.mineralItem == data)
        //    {
        //        slot.SetItem(data, inventoryDB.mineralCount.copperCount.Count);
        //        isAssigned = true;
        //        break;
        //    }
        //    else if (slot.mineralItem == data)
        //    {
        //        slot.SetItem(data, inventoryDB.mineralCount.ironCount.Count);
        //        isAssigned = true;
        //        break;
        //    }
        //}

        //foreach (Slot slot in slotDatas)
        //{
        //    if (slot.mineralItem != null && slot.mineralItem.itemName == "CopperItem")
        //    {
        //        Debug.Log("널 아닌 쿠퍼");
        //        slot.SetItem(data, inventoryDB.mineralCount.copperCount.Count);
        //        break;
        //    }
        //    else if (slot.mineralItem != null && slot.mineralItem.itemName == "IronItem")
        //    {
        //        Debug.Log("널 아닌 아이언");
        //        slot.SetItem(data, inventoryDB.mineralCount.ironCount.Count);
        //        break;
        //    }
        //}
    }
}