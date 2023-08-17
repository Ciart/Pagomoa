using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemHealEffect", menuName = "New ItemEffect/itemHungryEffect")]
public class HealHungryEffect : ItemEffect
{
    public int hungryHealValue;
    
    public override void Effect(ConsumerableItem item, Status stat)
    {
        stat.hungry += hungryHealValue;
        if(stat.hungry < stat.minHungry) stat.hungry = stat.minHungry;
        else if(stat.hungry > stat.maxHungry) stat.hungry = stat.maxHungry;
        stat.hungryAlter.Invoke(stat.hungry, stat.maxHungry);
    }
}
