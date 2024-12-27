using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class AuctionDB : MonoBehaviour
    {
        public List<InventorySlot> auctionItems = new List<InventorySlot>();

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
            var countUI = UIManager.instance.shopUI.GetCountUI();
            var auctionItem =UIManager.instance.shopUI.GetBuyUI().chosenBuySlot.slot;
            
            if (data.itemType == Item.ItemType.Use)
                for (int i = 0; i < countUI.inputCount; i++)
                {
                    gameManager.player.inventory.Gold -= data.itemPrice;
                }
            else if (data.itemType == Item.ItemType.Equipment)
            {
                gameManager.player.inventory.Gold -= data.itemPrice;
                auctionItem.count -= 1;
            }
            UIManager.instance.shopUI.playerGold[0].text = gameManager.player.inventory.Gold.ToString();
            UIManager.instance.shopUI.playerGold[1].text = gameManager.player.inventory.Gold.ToString();
        }
    }
}
