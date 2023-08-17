using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyNoCountUI : BuyCountUI
{
    public new void OnUI()
    {
        transform.gameObject.SetActive(true);
        price = Buy.Instance.choiceSlot.inventoryItem.item.itemPrice;
        totalPrice.text = price.ToString();
    }
}
