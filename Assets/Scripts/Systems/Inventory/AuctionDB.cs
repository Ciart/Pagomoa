using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class AuctionDB : MonoBehaviour
    {
        public List<InventorySlot> auctionItem = new List<InventorySlot>();

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
            // var inventoryItem = auctionItem.Find(inventoryItem => inventoryItem.item == data);
            var gameManager = GameManager.instance;
            var acutionItem = Buy.Instance.chosenBuySlot.slot;
            
            if (data.type == ItemType.Use)
                for (int i = 0; i < Buy.Instance.countUINum; i++)
                {
                    gameManager.player.inventory.Gold -= data.price;
                }
            else if (data.type == ItemType.Equipment)
            {
                gameManager.player.inventory.Gold -= data.price;
                acutionItem.count -= 1;
            }
            ShopUIManager.Instance.gold[0].text = gameManager.player.inventory.Gold.ToString();
            ShopUIManager.Instance.gold[1].text = gameManager.player.inventory.Gold.ToString();
        }
    }
}
