using Entities.Players;
using UnityEngine;

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
