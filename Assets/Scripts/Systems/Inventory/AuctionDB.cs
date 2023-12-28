using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class AuctionDB : MonoBehaviour
    {
        public List<InventoryItem> auctionItem = new List<InventoryItem>();

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
            var acutionItem = Buy.Instance.choiceSlot.inventoryItem;
            if (inventoryItem != null)
            {
                if (inventoryItem.item.itemType == Item.ItemType.Use)
                    for (int i = 0; i < BuyCountUI.Instance.count; i++)
                        InventoryDB.Instance.Gold -= data.itemPrice;
                else if (inventoryItem.item.itemType == Item.ItemType.Equipment)
                {
                    InventoryDB.Instance.Gold -= data.itemPrice;
                    acutionItem.count -= 1;
                }
                //if (inventoryItem.count == 0)
                //    auctionItem.Remove(inventoryItem);
            }
            Buy.Instance.gold.text = InventoryDB.Instance.Gold.ToString();
            //EtcInventory.Instance.ResetSlot();
            //Sell.Instance.ResetSlot();
            //InventoryDB.Instance.changeInventory.Invoke();
        }
        //public void CountDown()
        //{
        //    var Item = Buy.Instance.choiceSlot.inventoryItem;

        //    Item.count -= 1;
        //}
    }
}
