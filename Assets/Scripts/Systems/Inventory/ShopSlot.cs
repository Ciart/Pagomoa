using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class ShopSlot : InventorySlotUI
    {
        [SerializeField] private CountUI _countUI;

        public void SellCheck()
        {
            Sell.Instance.choiceSlot = this;

            var Inventory = Sell.Instance.choiceSlot.inventoryItem.item;
            if (Inventory == null)
                return;

            if (Inventory.itemType == Item.ItemType.Use ||
                Inventory.itemType == Item.ItemType.Mineral)
            {
                _countUI.OnUI(this.gameObject);
                ShopChat.Instance.SellPriceToChat(Inventory.itemPrice);
            }
        }

        public void ClickSlot()
        {
            for (int i = 0; i < _countUI.count; i++)
            {
                if (Sell.Instance.choiceSlot.inventoryItem.count > 1)
                {
                    InventoryDB.Instance.Remove(Sell.Instance.choiceSlot.inventoryItem.item);
                    QuickSlotItemDB.instance.SetCount(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                else if (Sell.Instance.choiceSlot.inventoryItem.count == 1)
                {
                    InventoryDB.Instance.Remove(Sell.Instance.choiceSlot.inventoryItem.item);
                    QuickSlotItemDB.instance.CleanSlot(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                Sell.Instance.DeleteSlot();
                Sell.Instance.ResetSlot();
            }
            _countUI.count = 0;
            _countUI.itemCount.text = _countUI.count.ToString();
            _countUI.OffUI();
        }
    }
}
