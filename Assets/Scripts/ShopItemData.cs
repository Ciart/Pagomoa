using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItemData", order = 2)]
public class ShopItemData : ScriptableObject
{
    public string itemName;

    public int price;

    public Sprite sprite;
}
