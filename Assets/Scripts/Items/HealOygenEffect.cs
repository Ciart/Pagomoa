using System.Collections;
using System.Collections.Generic;
using Entities.Players;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemHealEffect", menuName = "New ItemEffect/itemOxygenEffect")]
public class HealOygenEffect : ItemEffect
{
    public int oxygenHealValue;
    public override void Effect(ConsumerableItem item, PlayerStatus stat)
    {
        stat.oxygen += oxygenHealValue;
    }
}
