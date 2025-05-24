using System;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [Obsolete("OldItem is deprecated. Use Item instead.")]
    [CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
    public class OldItem : ScriptableObject
    {
        public enum OldItemType
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
