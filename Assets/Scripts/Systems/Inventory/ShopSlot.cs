using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Inventory;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class ShopSlot : Slot
    {
        public void SellCheck()
        {
            Buy.Instance.choosenSellSlot = this;

            var Inventory = Buy.Instance.choosenSellSlot.inventoryItem.item;
            if (Inventory == null)
                return;

            if (Inventory.itemType == Item.ItemType.Use ||
                Inventory.itemType == Item.ItemType.Mineral)
            {
                Buy.Instance.OnCountUI(this.gameObject);
                ShopChat.Instance.SellPriceToChat(Inventory.itemPrice);
            }
        }

        public void ClickSlot()
        {
            for (int i = 0; i < Buy.Instance.countUINum; i++)
            {
                if (Buy.Instance.choosenSellSlot.inventoryItem.count > 1)
                {
                    GameManager.player.inventoryDB.SellItem(Buy.Instance.choosenSellSlot.inventoryItem.item);
                    QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                else if (Buy.Instance.choosenSellSlot.inventoryItem.count == 1)
                {
                    GameManager.player.inventoryDB.SellItem(Buy.Instance.choosenSellSlot.inventoryItem.item);
                    QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
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
