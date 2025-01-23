using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ShopHover : Hover
    {
        [SerializeField] private Image edgeImage;
        private BuySlotUI buySlotUI =>  GetComponent<BuySlotUI>();
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            var shopUI = UIManager.instance.shopUI;
            var shopItemIDs = UIManager.instance.shopUI.GetShopItemIDs();
            var item = ResourceSystem.instance.GetItem(shopItemIDs[buySlotUI.slotID]);
            if (buySlotUI)
            {
                var itemName = item.name;
                shopUI.GetShopChat().chatting.text = itemName;
            }

            boostImage.sprite = hoverImage[0];
            if (edgeImage != null)
                edgeImage.gameObject.SetActive(true);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            boostImage.sprite = hoverImage[1];
            if (edgeImage != null)
                edgeImage.gameObject.SetActive(false);
        }
    }
}
