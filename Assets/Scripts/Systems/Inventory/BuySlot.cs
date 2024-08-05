using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlot : MonoBehaviour
    {
        [SerializeField] public InventorySlot slot;
        [SerializeField] public Image image;
        [SerializeField] public TextMeshProUGUI itemName;
        [SerializeField] public TextMeshProUGUI itemPrice;
        public void BuyCheck()
        {
            Buy.Instance.choosenBuySlot = this;
            var Shop = Buy.Instance.choosenBuySlot.slot.item;
            Buy.Instance.OnCountUI(this.gameObject);
            ShopChat.Instance.BuyPriceToChat(Shop.itemPrice);
        }
        public void UpdateConsumptionSlot()
        {
            image.sprite = slot.item.itemImage;
            itemName.text = slot.item.itemName;
            itemPrice.text = slot.item.itemPrice.ToString();
        }
    }
}