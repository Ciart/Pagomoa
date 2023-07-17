using Maps;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class EtcInventory : MonoBehaviour
{
    [SerializeField] private InventoryDB inventoryDB;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject slot;
    [SerializeField] public  List<Slot> slotDatas = new List<Slot>();
    [SerializeField] private int count;

    private static EtcInventory instance;
    public static EtcInventory Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(EtcInventory)) as EtcInventory;
            }
            return instance;
        }
    }
    public void makeSlot() // slotDatas 갯수만큼 슬롯 만들기
    {
        for (int i = 0; i < count; i++)
        {
            GameObject SpawnedSlot = Instantiate(slot, slotParent.transform);
            slotDatas.Add(SpawnedSlot.GetComponent<Slot>());
            SpawnedSlot.SetActive(true);
        }
    }
    public void resetSlot() // 각각 슬롯에 item 할당
    {
        int i = 0;
        for(; i < inventoryDB.items.Count && i < slotDatas.Count; i++)
        {
            slotDatas[i].item = inventoryDB.items[i].item;
        }
        for(; i< slotDatas.Count; i++)
        {
            slotDatas[i].item = null;
        }
        UpdateSlot();
    }
    public void UpdateSlot() // List안의 Item 전체 인벤토리에 출력
    {
        for(int i = 0; i < inventoryDB.items.Count; i++)
        {
            slotDatas[i].SetUI(inventoryDB.items[i].item.itemImage, inventoryDB.items[i].count.ToString());
        }
    }
    public void DeleteSlot() // 인벤토리에 출력된 아이템들 전부 NULL
    { 
        for (int i = 0; i < inventoryDB.items.Count; i++)
        {
            slotDatas[i].SetUI(null, "");
        }
    }
}