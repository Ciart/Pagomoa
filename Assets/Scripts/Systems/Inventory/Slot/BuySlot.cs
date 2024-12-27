using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlot : MonoBehaviour
    {
        [SerializeField] private ShopChat shopChat;

        [SerializeField] public InventorySlot slot;
        [SerializeField] public Image image;
        [SerializeField] public TextMeshProUGUI itemName;
        [SerializeField] public TextMeshProUGUI itemPrice;

        public SlotType slotType = SlotType.Buy;
        
        public void BuyCheck()
        {
            UIManager.instance.shopUI.GetBuyUI().chosenBuySlot = this;
            var chosenItem = UIManager.instance.shopUI.GetBuyUI().chosenBuySlot.slot.item;
            
            UIManager.instance.shopUI.GetCountUI().gameObject.SetActive(true);
            UIManager.instance.shopUI.GetCountUI().ActiveCountUI(slotType);
            UIManager.instance.shopUI.GetShopChat().BuyPriceToChat(chosenItem.price);
        }
        public void UpdateConsumptionSlot()
        {
            image.sprite = slot.item.sprite;
            itemName.text = slot.item.name;
            itemPrice.text = slot.item.price.ToString();
        }
    }
}