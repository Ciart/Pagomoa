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
            buy.chosenSellSlot = this;

            var Inventory = buy.chosenSellSlot.slot.item;
            if (Inventory == null)
                return;

            if (Inventory.type == ItemType.Use ||
                Inventory.type == ItemType.Mineral)
            {
                buy.OnCountUI(this.gameObject);
                ShopChat.Instance.SellPriceToChat(Inventory.price);
            }
        }

        public void ClickSlot()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < buy.countUINum; i++)
            {
                if (buy.chosenSellSlot.slot.count > 1)
                {
                    inventory.SellItem(buy.chosenSellSlot.slot.item);
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                else if (buy.chosenSellSlot.slot.count == 1)
                {
                    inventory.SellItem(buy.chosenSellSlot.slot.item);
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                buy.DeleteSellUISlot();
                buy.ResetSellUISlot();
            }
            buy.countUINum = 0;
            buy.countUIText.text = buy.countUINum.ToString();
            buy.OffCountUI();
        }
    }
}
