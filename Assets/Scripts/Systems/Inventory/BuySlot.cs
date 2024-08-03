using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlot : MonoBehaviour
    {
        [FormerlySerializedAs("inventoryItem")] [SerializeField] public InventorySlot slot;
        [SerializeField] public Image image;
        [SerializeField] private CountUI _countUI;
        [SerializeField] public TextMeshProUGUI itemName;
        [SerializeField] public TextMeshProUGUI itemPrice;
        public void BuyCheck()
        {
            Buy.Instance.choiceSlot = this;
            var Shop = Buy.Instance.choiceSlot.slot.item;
            _countUI.OnUI(this.gameObject);
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