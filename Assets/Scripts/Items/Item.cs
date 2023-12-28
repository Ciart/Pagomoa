using System.Collections;
using System.Collections.Generic;
using Entities.Players;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Use,
        Mineral,
        Inherent
    }
    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;
    public string itemInfo;
    public int itemPrice;

    public virtual void Active(PlayerStatus stat) { }
}
