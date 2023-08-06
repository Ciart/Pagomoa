using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopConsumptionItem", menuName = "ScriptableObjects/ShopConsumptionItemData", order = 3)]
public class ShopConsumptionItemData : ScriptableObject { 

    public string ConsumptionItemname;

    public int price;

    public Sprite sprite;
}
