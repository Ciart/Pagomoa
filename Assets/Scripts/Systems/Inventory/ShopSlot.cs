using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Inventory;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class ShopSlot : InventorySlotUI
    {
        public void SellCheck()
        {
            Buy.Instance.chosenSellSlot = this;

            var Inventory = Buy.Instance.chosenSellSlot.slot.item;
            if (Inventory == null)
                return;

            if (Inventory.type == ItemType.Use ||
                Inventory.type == ItemType.Mineral)
            {
                Buy.Instance.OnCountUI(this.gameObject);
                ShopChat.Instance.SellPriceToChat(Inventory.price);
            }
        }

        public void ClickSlot()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < Buy.Instance.countUINum; i++)
            {
                if (Buy.Instance.chosenSellSlot.slot.count > 1)
                {
                    inventory.SellItem(Buy.Instance.chosenSellSlot.slot.item);
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                else if (Buy.Instance.chosenSellSlot.slot.count == 1)
                {
                    inventory.SellItem(Buy.Instance.chosenSellSlot.slot.item);
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                Buy.Instance.DeleteSellUISlot();
                Buy.Instance.ResetSellUISlot();
            }
            Buy.Instance.countUINum = 0;
            Buy.Instance.countUIText.text = Buy.Instance.countUINum.ToString();
            Buy.Instance.OffCountUI();
        }
    }
}
