using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
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
                if (data.itemType == Item.ItemType.Use)
                    for (int i = 0; i < CountUI.Instance.count; i++)
                    {
                        InventoryDB.Instance.Gold -= data.itemPrice;
                    }
                else if (data.itemType == Item.ItemType.Equipment)
                {
                    InventoryDB.Instance.Gold -= data.itemPrice;
                    acutionItem.count -= 1;
                }
            }
            ShopUIManager.Instance.gold[0].text = InventoryDB.Instance.Gold.ToString();
            ShopUIManager.Instance.gold[1].text = InventoryDB.Instance.Gold.ToString();
        }
    }
}
