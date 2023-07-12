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
    [SerializeField] private GameObject slots;
    [SerializeField] private GameObject slot;
    public int slotCount;

    private void Awake()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject SpawnedSlot = Instantiate(slot, slots.transform);
            SpawnedSlot.SetActive(true);
        }
    }
    public void InputSlot(Mineral data, int id)
    {
        Slot[] slotDatas = slots.GetComponentsInChildren<Slot>();
        foreach (Slot slot in slotDatas)
        {
            if (slot.id == id)
                slot.SetItem(data.item, inventoryDB.MineralsData[data]);
        }
    }
    public void InputSlot(Mineral data)
    {
        Slot[] slotDatas = slots.GetComponentsInChildren<Slot>();

       
        foreach (Slot slot in slotDatas)
        {
            if (slot.mineralItem == null)
            {
                if (data.item.itemName == "CopperItem")
                {
                    Debug.Log("±¸¸®");
                    slot.SetItem(data.item, inventoryDB.mineralCount.copperCount.Count);
                    break;
                }
                else if (data.item.itemName == "IronItem")
                {
                    Debug.Log("Àº");
                    slot.SetItem(data.item, inventoryDB.mineralCount.ironCount.Count);
                    break;
                }
            }
            else
            {
                if (slot.mineralItem.itemName == "CopperItem")
                    slot.RemoveItem();
                else if (slot.mineralItem.itemName == "IronItem")
                    slot.RemoveItem();
            }
        }
        //bool isAssigned = false;
        //foreach (Slot slot in slotDatas)
        //{
        //    if(slot.mineralItem == data)
        //    {
        //        slot.SetItem(data.item, inventoryDB.mineralCount.copperCount.Count);
        //        isAssigned = true;
        //        break;
        //    }
        //    else if(slot.mineralItem == data)
        //    {
        //        slot.SetItem(data.item, inventoryDB.mineralCount.ironCount.Count);
        //        isAssigned = true;
        //        break;
        //    }
        //}
        //if (!isAssigned)
        //{
        //    foreach (Slot slot in slotDatas)
        //    {
        //        if (slot.mineralItem == null)
        //        {
        //            slot.SetItem(data.item, inventoryDB.mineralCount.copperCount.Count);
        //            break;
        //        }
        //        else if (slot.mineralItem == null)
        //        {
        //            slot.SetItem(data.item, inventoryDB.mineralCount.ironCount.Count);
        //            break;
        //        }
        //    }
        //}
    }
}