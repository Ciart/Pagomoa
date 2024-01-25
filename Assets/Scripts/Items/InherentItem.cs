using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "New Item/Inherent item")]

    public class InherentItem : Item
    {
        public bool Usable;
        public List<InherentEffect> Effects;

        public override void Active(PlayerStatus stat)
        {
            Use(stat);
        }

        public virtual void Use(PlayerStatus stat)
        {
            foreach (InherentEffect effect in Effects)
            {
                effect.Effect(this, stat);
            }
        }
    }
}