using System;
using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlot : MonoBehaviour
    {
        public Slot buySlot { get; private set; } = new Slot();
        
        [Header("구매 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemPriceText;

        private void Awake()
        {
            buySlot.SetSlotType(SlotType.Buy);
            SetCountBuySlot();
        }

        public void BuyCheck()
        {
            var shopUI = UIManager.instance.shopUI;
            
            shopUI.chosenSlot = buySlot;
            var chosenItem = shopUI.chosenSlot.GetSlotItem();
            
            shopUI.GetCountUI().gameObject.SetActive(true);
            shopUI.GetCountUI().ActiveCountUI(buySlot.GetSlotType());
            shopUI.GetShopChat().BuyPriceToChat(chosenItem.price);
        }
        public virtual void UpdateBuySlot()
        {
            itemImage.sprite = buySlot.GetSlotItem().sprite;
            itemNameText.text = buySlot.GetSlotItem().name;
            itemPriceText.text = buySlot.GetSlotItem().price.ToString();
        }

        protected void SetCountBuySlot()
        {
            if (buySlot.GetSlotType() == SlotType.Buy)
                buySlot.SetSlotItemCount(100000);
            else if (buySlot.GetSlotType() == SlotType.BuyArtifact)
                buySlot.SetSlotItemCount(1);
        }
    }
}