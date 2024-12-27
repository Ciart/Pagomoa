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
            var player = GameManager.instance.player;
            var countUI = UIManager.instance.shopUI.GetCountUI();
            var auctionItem = UIManager.instance.shopUI.GetBuyUI().chosenBuySlot.slot;
            
            if (data.type == ItemType.Use)
                for (int i = 0; i < countUI.inputCount; i++)
                {
                    player.inventory.Gold -= data.price;
                }
            else if (data.type == ItemType.Equipment)
            {
                player.inventory.Gold -= data.price;
                auctionItem.count -= 1;
            }
            UIManager.instance.shopUI.playerGold[0].text = player.inventory.Gold.ToString();
            UIManager.instance.shopUI.playerGold[1].text = player.inventory.Gold.ToString();
        }
    }
}
