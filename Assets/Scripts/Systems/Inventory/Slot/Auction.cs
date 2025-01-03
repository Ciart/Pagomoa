using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Auction : MonoBehaviour
    {
        public List<string> auctionItemsID = new();
        private List<Item> _auctionItems;
        public List<Item> GetAuctionItems() => _auctionItems;


        private void Start()
        {
            _auctionItems = new List<Item>();
            
            foreach (var id in auctionItemsID)
            {
                _auctionItems.Add(ResourceSystem.instance.GetItem(id));
            }
        }
        
        public void Remove(Item data)
        {
            var player = GameManager.instance.player;
            var countUI = UIManager.instance.shopUI.GetCountUI();
            var auctionItem = UIManager.instance.shopUI.chosenSlot;
            
            if (data.type == ItemType.Use)
                for (int i = 0; i < countUI.inputCount; i++)
                {
                    player.inventory.Gold -= data.price;
                }
            else if (data.type == ItemType.Equipment)
            {
                var minusCount = auctionItem.GetSlotItemCount() - 1; 
                
                player.inventory.Gold -= data.price;
                auctionItem.SetSlotItemCount(minusCount);
            }
        }
    }
}
