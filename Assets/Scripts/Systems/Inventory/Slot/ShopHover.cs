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
        private BuySlot _buySlot;

        private void Start()
        {
            _buySlot = GetComponent<BuySlot>();
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            var shopUI = UIManager.instance.shopUI;
            
            if (_buySlot)
            {
                var itemName = _buySlot.GetSlotItem().name;
                shopUI.GetShopChat().chatting.text = itemName;
            }
            
            shopUI.hovering = this;

            boostImage.sprite = hoverImage[0];
            if (edgeImage != null)
                edgeImage.gameObject.SetActive(true);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            UIManager.instance.shopUI.hovering = null;
            boostImage.sprite = hoverImage[1];
            if (edgeImage != null)
                edgeImage.gameObject.SetActive(false);
        }
    }
}
