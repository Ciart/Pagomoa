﻿using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlot : MonoBehaviour
    {
        [SerializeField] private Buy buy;
        [SerializeField] private ShopChat shopChat;

        [SerializeField] public InventorySlot slot;
        [SerializeField] public Image image;
        [SerializeField] public TextMeshProUGUI itemName;
        [SerializeField] public TextMeshProUGUI itemPrice;

        public void BuyCheck()
        {
            buy.chosenBuySlot = this;
            var Shop = buy.chosenBuySlot.slot.item;
            buy.OnCountUI(this.gameObject);
            shopChat.BuyPriceToChat(Shop.price);
        }
        public void UpdateConsumptionSlot()
        {
            image.sprite = slot.item.sprite;
            itemName.text = slot.item.name;
            itemPrice.text = slot.item.price.ToString();
        }
    }
}
