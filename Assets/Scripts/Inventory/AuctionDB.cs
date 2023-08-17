using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class AuctionDB : MonoBehaviour
{
    public List<InventoryItem> auctionItem = new List<InventoryItem>();
    public EtcInventory inventory;
    public Sell sell;

    private static AuctionDB instance;
    public static AuctionDB Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(AuctionDB)) as AuctionDB;
            }
            return instance;
        }
    }
    public void Remove(Item data)
    {
        var inventoryItem = auctionItem.Find(inventoryItem => inventoryItem.item == data);
        if (inventoryItem != null)
        {
            if (inventoryItem.item.itemType == Item.ItemType.Use)
                for (int i = 0; i < BuyCountUI.Instance.count; i++)
                    InventoryDB.Instance.Gold -= data.itemPrice;
            else
                InventoryDB.Instance.Gold -= data.itemPrice;

            if (inventoryItem.count == 0)
                auctionItem.Remove(inventoryItem);
        }
        Buy.Instance.gold.GetComponent<Text>().text = InventoryDB.Instance.Gold.ToString();
        inventory.ResetSlot();
        sell.ResetSlot();
    }
}
