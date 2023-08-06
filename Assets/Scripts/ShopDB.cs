using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopDB : MonoBehaviour
{
    public List<ShopItemData> items = new List<ShopItemData>();
    public List<ShopConsumptionItemData> consumptionitems = new List<ShopConsumptionItemData>();
    public InventoryDB inventoryDB;
    public void RemoveItem(ShopItemData data)
    {
        if (data.price <= inventoryDB.Gold)
        {
            items.Remove(data);
            inventoryDB.Gold -= data.price;
            inventoryDB.AddEquipmentItemData(data);
        }
    }
    public void RemoveConsumptionItem(ShopConsumptionItemData data)
    {
        //consumptionitems.Remove(data);
        inventoryDB.Gold -= data.price;
        inventoryDB.AddConsumptionItemData(data);
    }
}
