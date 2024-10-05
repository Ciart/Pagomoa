using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
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

        // TODO: stat 삭제하거나 구조를 변경해야 합니다.
        public virtual void Active(PlayerStatus stat) { }
    }
}
