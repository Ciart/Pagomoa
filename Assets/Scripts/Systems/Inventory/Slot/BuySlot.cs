using System;
using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlot : Slot
    {
        public Image itemImage;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemPriceText;

        private void Awake()
        {
            SetSlotType(SlotType.Buy);
            SetCountBuySlot();
        }

        public void BuyCheck()
        {
            var shopUI = UIManager.instance.shopUI;
            
            shopUI.chosenSlot = this;
            var chosenItem = shopUI.chosenSlot.GetSlotItem();
            
            shopUI.GetCountUI().gameObject.SetActive(true);
            shopUI.GetCountUI().ActiveCountUI(GetSlotType());
            shopUI.GetShopChat().BuyPriceToChat(chosenItem.price);
        }
        public virtual void UpdateBuySlot()
        {
            itemImage.sprite = GetSlotItem().sprite;
            itemNameText.text = GetSlotItem().name;
            itemPriceText.text = GetSlotItem().price.ToString();
        }

        protected void SetCountBuySlot()
        {
            if (GetSlotType() == SlotType.Buy)
                SetSlotItemCount(100000);
            else if (GetSlotType() == SlotType.BuyArtifact)
                SetSlotItemCount(1);
        }
    }
}