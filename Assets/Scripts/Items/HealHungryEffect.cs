using System;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [Obsolete("Ciart.Pagomoa.Items.ItemEffectLsit를 확인해주세요.")]
    [CreateAssetMenu(fileName = "New ItemHealEffect", menuName = "New ItemEffect/itemHungryEffect")]
    public class HealHungryEffect : ItemEffect
    {
        public int hungryHealValue;
    
        public override void Effect(ConsumableItem item, PlayerStatus stat)
        {
            /*stat.Hungry += hungryHealValue;
            if(stat.Hungry < stat.minHungry) stat.Hungry = stat.minHungry;
            else if(stat.Hungry > stat.maxHungry) stat.Hungry = stat.maxHungry;
            stat.hungryAlter.Invoke(stat.Hungry, stat.maxHungry);*/
        }
    }
}
