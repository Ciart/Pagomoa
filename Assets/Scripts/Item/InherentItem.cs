using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Inherent item")]

public class InherentItem : Item
{
    public List<ItemInherentEffect> inherentEffects;

    public override void Active(Status stat)
    {
        Use(stat);
    }

    protected virtual void Use(Status stat)
    {
        foreach(ItemInherentEffect effect in inherentEffects)
        {
            effect.InherentEffect(this, stat);
        }
    }
}