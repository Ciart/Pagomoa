using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyCountUI : MonoBehaviour
{
    [SerializeField] private Image item;
    [SerializeField] public TextMeshProUGUI itemCount;
    [SerializeField] public TextMeshProUGUI totalPrice;
    [SerializeField] public int price = 0;
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
        price = 0;
        count = 0;
        totalPrice.text = price.ToString();
        itemCount.text = count.ToString();
    }
    public void OffUI()
    {
        transform.gameObject.SetActive(false);
    }
    public void ItemImage(Sprite image)
    {
        item.sprite = image;
    }
    public void Plus()
    {
        InventoryItem item = Buy.Instance.choiceSlot.inventoryItem;
        if (count < item.count)
        {
            count++;
            price += item.item.info.itemPrice;
        }
        else
            return;
        totalPrice.text = price.ToString();
        itemCount.text = count.ToString();
    }
    public void Minus()
    {
        {
            if (count > 0)
            {
                count--;
                price -= Buy.Instance.choiceSlot.inventoryItem.item.info.itemPrice;
            }
            else
                return;
            totalPrice.text = price.ToString();
            itemCount.text = count.ToString();
        }
    }
}
