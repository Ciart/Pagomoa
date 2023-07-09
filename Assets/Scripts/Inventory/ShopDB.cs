using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopDB : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<Item> consumptionitems = new List<Item>();
    public InventoryDB inventoryDB;
    public void RemoveItem(Item data)
    {
        if (data.itemPrice <= inventoryDB.Gold)
        {
            items.Remove(data);
            inventoryDB.Gold -= data.itemPrice;
            inventoryDB.AddEquipmentItemData(data);
        }
    }
    public void RemoveConsumptionItem(Item data)
    {
        //consumptionitems.Remove(data);
        inventoryDB.Gold -= data.itemPrice;
        inventoryDB.AddConsumptionItemData(data);
    }
}
