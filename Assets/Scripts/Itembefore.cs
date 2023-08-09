using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/itembefore")]
public class Itembefore : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Use,
        Mineral,
        Other,
    }
    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;
    public string itemInfo;
    public int itemPrice;
}
