using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Inherent item")]

public class InherentItem : Item
{
    public bool Usable;
    public List<InherentEffect> Effects;

    public override void Active(Status stat)
    {
        Use(stat);
    }

    public virtual void Use(Status stat)
    {
        foreach (InherentEffect effect in Effects)
        {
            effect.Effect(this, stat);
        }
    }
}