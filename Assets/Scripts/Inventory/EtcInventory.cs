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
    public void makeSlot() // slotDatas ������ŭ ���� �����
    {
        for (int i = 0; i < count; i++)
        {
            GameObject SpawnedSlot = Instantiate(slot, slotParent.transform);
            slotDatas.Add(SpawnedSlot.GetComponent<Slot>());
            SpawnedSlot.SetActive(true);
        }
    }
    public void resetSlot() // ���� ���Կ� item �Ҵ�
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
    public void UpdateSlot() // List���� Item ��ü �κ��丮�� ���
    {
        for(int i = 0; i < inventoryDB.items.Count; i++)
        {
            slotDatas[i].SetUI(inventoryDB.items[i].item.itemImage, inventoryDB.items[i].count.ToString());
        }
    }
    public void DeleteSlot() // �κ��丮�� ��µ� �����۵� ���� NULL
    { 
        for (int i = 0; i < inventoryDB.items.Count; i++)
        {
            slotDatas[i].SetUI(null, "");
        }
    }
}