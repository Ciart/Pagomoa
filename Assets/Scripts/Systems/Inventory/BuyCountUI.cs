using Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyCountUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI itemCount;
    [SerializeField] public int count = 0;

    private static BuyCountUI instance;
    public static BuyCountUI Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(BuyCountUI)) as BuyCountUI;
            }
            return instance;
        }
    }
    public void OnUI()
    {
        transform.gameObject.SetActive(true);
        count = 0;
        itemCount.text = count.ToString();
    }
    public void OffUI()
    {
        ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
        ShopChat.Instance.CancleChat();
        transform.gameObject.SetActive(false);
    }
    
    public void Plus()
    {
        InventoryItem item = Buy.Instance.choiceSlot.inventoryItem;
        if (item.item.itemType == Item.ItemType.Use)
        {
            count++;
            ShopChat.Instance.TotalPriceToChat(count * item.item.itemPrice);
        }

        else if(item.item.itemType == Item.ItemType.Equipment || item.item.itemType == Item.ItemType.Inherent)
        {
            if (count < item.count)
                count++;
            else
                return;
        }
        else
            return;
        itemCount.text = count.ToString();
    }
    public void Minus()
    {
        {
            if (count > 0)
            {
                count--;
            }
            else
                return;
            itemCount.text = count.ToString();
        }
    }
    public void BuySlots()
    {
        var Shop = Buy.Instance.choiceSlot.inventoryItem;
        if (Shop.item.itemType == Item.ItemType.Use)
        {
            if (InventoryDB.Instance.Gold >= Shop.item.itemPrice * count && count > 0)
            {
                InventoryDB.Instance.Add(Shop.item, count);
                AuctionDB.Instance.Remove(Shop.item);
                ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
                OffUI();
            }
            else
                return;

            ShopChat.Instance.ThakChat();
        }

        else if (Shop.item.itemType == Item.ItemType.Equipment || Shop.item.itemType == Item.ItemType.Inherent)
        {
            if (InventoryDB.Instance.Gold >= Shop.item.itemPrice && Shop.count == count)
            {
                InventoryDB.Instance.Add(Shop.item, 0);
                AuctionDB.Instance.Remove(Shop.item);
                Buy.Instance.UpdateCount();
                Buy.Instance.SoldOut();
                ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
                OffUI();
            }
            else
                return;
            ShopChat.Instance.ThakChat();
        }
    }
}
