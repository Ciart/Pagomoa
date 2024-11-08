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
            Buy.Instance.chosenBuySlot = this;
            var Shop = Buy.Instance.chosenBuySlot.slot.item;
            Buy.Instance.OnCountUI(this.gameObject);
            ShopChat.Instance.BuyPriceToChat(Shop.price);
        }
        public void UpdateConsumptionSlot()
        {
            image.sprite = slot.item.sprite;
            itemName.text = slot.item.name;
            itemPrice.text = slot.item.price.ToString();
        }
    }
}