using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "New Item/Consumerable item")]
    public class ConsumerableItem : OldItem
    {
        public bool Usable;
        public List<ItemEffect> Effects;

        public override void Active(PlayerStatus stat)
        {
            Use(stat);
        }

        public virtual void Use(PlayerStatus stat)
        {
            foreach(ItemEffect effect in Effects)
            {
                effect.Effect(this, stat);
            }
        }
    }
}
