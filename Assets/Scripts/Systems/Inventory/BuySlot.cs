using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlot : MonoBehaviour
    {
        [SerializeField] public InventoryItem inventoryItem;
        [SerializeField] public Image image;
        [SerializeField] public TextMeshProUGUI itemName;
        [SerializeField] public TextMeshProUGUI itemPrice;
        public void BuyCheck()
        {
            Buy.Instance.choosenBuySlot = this;
            var Shop = Buy.Instance.choosenBuySlot.inventoryItem.item;
            Buy.Instance.OnCountUI(this.gameObject);
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