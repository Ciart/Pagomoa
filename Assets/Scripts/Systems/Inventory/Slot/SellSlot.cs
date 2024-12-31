using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using UnityEngine;


namespace Ciart.Pagomoa
{
    public class SellSlot : Slot
    { 
        public void SellCheck()
        {
            var shopUI = UIManager.instance.shopUI;
            
            shopUI.chosenSlot = this;

            var inventory = shopUI.chosenSlot.GetSlotItem();
            if (inventory == null)
                return;

            if (inventory.type == ItemType.Use ||
                inventory.type == ItemType.Mineral)
            {
                UIManager.instance.shopUI.GetCountUI().gameObject.SetActive(true);
                UIManager.instance.shopUI.GetCountUI().ActiveCountUI(GetSlotType());
                UIManager.instance.shopUI.GetShopChat().SellPriceToChat(inventory.price);
            }
        }

        public void ClickSlot()
        {
            var inventory = GameManager.instance.player.inventory;
            var shopUI = UIManager.instance.shopUI;
            var countUI = shopUI.GetCountUI();
            
            for (int i = 0; i < countUI.inputCount; i++)
            {
                if (shopUI.chosenSlot.GetSlotItemCount() > 1)
                {
                    inventory.SellItem(shopUI.chosenSlot.GetSlotItem());
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                else if (shopUI.chosenSlot.GetSlotItemCount() == 1)
                {
                    inventory.SellItem(shopUI.chosenSlot.GetSlotItem());
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                shopUI.GetSellUI().DeleteSellUISlot();
                shopUI.GetSellUI().ResetSellUISlot();
            }
            
            countUI.inputCount = 0;
            countUI.countUIText.text = countUI.inputCount.ToString();
            countUI.DisableCountUI();
        }
    }
}
