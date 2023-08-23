using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemHealEffect", menuName = "New ItemEffect/itemOxygenEffect")]
public class HealOygenEffect : ItemEffect
{
    public int oxygenHealValue;
    public override void Effect(ConsumerableItem item, Status stat)
    {
        stat.oxygen += oxygenHealValue;
    }
}
