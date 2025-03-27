using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    public abstract class ItemEffect : ScriptableObject
    {
        public abstract void Effect(ConsumableItem item, PlayerStatus stat);
    }
}
