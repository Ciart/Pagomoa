using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Consumerable item")]
public class ConsumerableItem : Item
{
    public bool Usable;
    public List<ItemEffect> Effects;

    public override void Active(Status stat)
    {
        Use(stat);
    }

    public virtual void Use(Status stat)
    {
        foreach(ItemEffect effect in Effects)
        {
            effect.Effect(this, stat);
        }
    }
}
