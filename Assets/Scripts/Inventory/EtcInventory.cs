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
    public void InputSlot(Mineral data, int id)
    {
        Slot[] slotDatas = slots.GetComponentsInChildren<Slot>();
        foreach (Slot slot in slotDatas)
        {
            if (slot.id == id)
                slot.SetItem(data, inventoryDB.MineralsData[data]);
        }
    }
    public void InputSlot(Mineral data)
    {
        Slot[] slotDatas = slots.GetComponentsInChildren<Slot>();
        bool isAssigned = false;
        foreach (Slot slot in slotDatas)
        {
            if(slot.mineral == data)
            {
                slot.SetItem(data, inventoryDB.MineralsData[data]);
                isAssigned = true;
                break;
            }
        }
        if (!isAssigned)
        {
            foreach (Slot slot in slotDatas)
            {
                if (slot.mineral == null)
                {
                    slot.SetItem(data, inventoryDB.MineralsData[data]);
                    break;
                }
            }
        }
    } 
}