using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Ciart.Pagomoa
{
    public class SellSlot : Slot
    {
        [Header("판매 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI countText;
        public int id;
        
        private void Awake()
        {
            SetSlotType(SlotType.Sell);
        }
        
        public override void SetSlot(Item setItem)
        {
            if (setItem.id != "")
            {
                itemImage.sprite = setItem.sprite;
                countText.text = GetSlotItemCount() == 0 ? "" : GetSlotItemCount().ToString();
            }
            else
            {
                itemImage.sprite = null;
                countText.text = "";
            } 
        }

        public void ResetSlot()
        {
            itemImage.sprite = null;
            countText.text = "";
        }
        
        public void SellCheck()
        {
            var shopUI = UIManager.instance.shopUI;
            
            shopUI.chosenSlot = this;

            var inventoryItem = shopUI.chosenSlot.GetSlotItem();
            if (inventoryItem.id == "")
                return;

            if (inventoryItem.type == ItemType.Use ||
                inventoryItem.type == ItemType.Mineral)
            {
                UIManager.instance.shopUI.GetCountUI().gameObject.SetActive(true);
                UIManager.instance.shopUI.GetCountUI().ActiveCountUI(GetSlotType());
                UIManager.instance.shopUI.GetShopChat().SellPriceToChat(inventoryItem.price);
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
                shopUI.GetSellUI().UpdateSellUISlot();
            }
            
            countUI.inputCount = 0;
            countUI.countUIText.text = countUI.inputCount.ToString();
            countUI.DisableCountUI();
        }
    }
}
