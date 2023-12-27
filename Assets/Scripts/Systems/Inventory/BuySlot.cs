using Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuySlot : Slot
{
    [SerializeField] public GameObject _soldOut;

    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemPrice;
    [SerializeField] public TextMeshProUGUI itemNum;

    public void BuyCheck()
    {
        Buy.Instance.choiceSlot = this;

        var Shop = Buy.Instance.choiceSlot.inventoryItem.item;
        buyCountUI.OnUI();
        ShopChat.Instance.BuyPriceToChat(Shop.itemPrice);
    }
    
    public void UpdateArtifactSlot()
    {
        image.sprite = inventoryItem.item.itemImage;
        _itemName.text = inventoryItem.item.itemName;
        _itemPrice.text = inventoryItem.item.itemPrice.ToString();
        itemNum.text = inventoryItem.count.ToString();
    }
    public void UpdateConsumptionSlot()
    {
        image.sprite = inventoryItem.item.itemImage;
        _itemName.text = inventoryItem.item.itemName;
        _itemPrice.text = inventoryItem.item.itemPrice.ToString();
    }
}
