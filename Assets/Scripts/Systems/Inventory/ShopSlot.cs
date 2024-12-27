using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Systems;


namespace Ciart.Pagomoa
{
    public class ShopSlot : InventorySlotUI
    {
        private void Start() { slotType = SlotType.Shop; }

        public void SellCheck()
        {
            var buyUI = UIManager.instance.shopUI.GetBuyUI();
            
            buyUI.chosenSellSlot = this;

            var inventory = buyUI.chosenSellSlot.slot.item;
            if (inventory == null)
                return;

            if (inventory.type == ItemType.Use ||
                inventory.type == ItemType.Mineral)
            {
                UIManager.instance.shopUI.GetCountUI().gameObject.SetActive(true);
                UIManager.instance.shopUI.GetCountUI().ActiveCountUI(slotType);
                UIManager.instance.shopUI.GetShopChat().SellPriceToChat(inventory.price);
            }
        }

        public void ClickSlot()
        {
            var inventory = GameManager.instance.player.inventory;
            var countUI = UIManager.instance.shopUI.GetCountUI();
            var buyUI = UIManager.instance.shopUI.GetBuyUI();
            
            for (int i = 0; i < countUI.inputCount; i++)
            {
                if (buyUI.chosenSellSlot.slot.count > 1)
                {
                    inventory.SellItem(buyUI.chosenSellSlot.slot.item);
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                else if (buyUI.chosenSellSlot.slot.count == 1)
                {
                    inventory.SellItem(buyUI.chosenSellSlot.slot.item);
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                buyUI.DeleteSellUISlot();
                buyUI.ResetSellUISlot();
            }
            countUI.inputCount = 0;
            countUI.countUIText.text = countUI.inputCount.ToString();
            countUI.DisableCountUI();
        }
    }
}
