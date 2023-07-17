using System;
using System.Collections;
using System.Collections.Generic;
using Maps;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.UI;
using static UnityEditor.Progress;
public class InventoryDB : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public int Gold;
    [SerializeField] private EtcInventory inventory;

    public UnityEvent makeSlots;
    public UnityEvent changeInventory;

    private static InventoryDB instance;
    public static InventoryDB Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(InventoryDB)) as InventoryDB;
            }
            return instance;
        }
    }
    private void Awake()
    {
        makeSlots.Invoke();
    }
    public void AddMineral(Mineral data)
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data.item);

        if (inventoryItem != null)
        {
            inventoryItem.count++;
        }
        else
        {
            items.Add(new InventoryItem(data.item, 1));
        }
        changeInventory.Invoke();
    }
    public void Remove(Item data)
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
        if (inventoryItem != null)
        {
            if (inventoryItem.count > 1)
                inventoryItem.count--;
            else if (inventoryItem.count == 1)
                items.Remove(inventoryItem);
        }
        changeInventory.Invoke();
    }
}