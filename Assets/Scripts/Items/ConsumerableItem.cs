using System.Collections;
using System.Collections.Generic;
using Entities.Players;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Consumerable item")]
public class ConsumerableItem : Item
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
