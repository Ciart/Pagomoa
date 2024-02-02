using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class BuySlot : MonoBehaviour
    {
        [SerializeField] public InventoryItem inventoryItem;
        [SerializeField] public Image image;
        [SerializeField] private CountUI _countUI;
        [SerializeField] public TextMeshProUGUI itemName;
        [SerializeField] public TextMeshProUGUI itemPrice;
        public void BuyCheck()
        {
            Buy.Instance.choiceSlot = this;

            var Shop = Buy.Instance.choiceSlot.inventoryItem.item;
            _countUI.OnUI(this.gameObject);
            ShopChat.Instance.BuyPriceToChat(Shop.itemPrice);
        }
        public void UpdateConsumptionSlot()
        {
            image.sprite = inventoryItem.item.itemImage;
            itemName.text = inventoryItem.item.itemName;
            itemPrice.text = inventoryItem.item.itemPrice.ToString();
        }
    }
}
