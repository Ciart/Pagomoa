using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopHover : Hover
{
    [SerializeField] private Image _edgeImage;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<BuySlot>())
        {
            var ItemName = this.GetComponent<BuySlot>().inventoryItem.item.itemName;
            ShopChat.Instance.chatting.text = ItemName;
        }

        ShopUIManager.Instance.hovering = this;

        boostImage.sprite = hoverImage[0];
        if (_edgeImage != null)
            _edgeImage.gameObject.SetActive(true);
        else
            return;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        ShopUIManager.Instance.hovering = null;
        boostImage.sprite = hoverImage[1];
        if (_edgeImage != null)
            _edgeImage.gameObject.SetActive(false);
        else
            return;
    }
}