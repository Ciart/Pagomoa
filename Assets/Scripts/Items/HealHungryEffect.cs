using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [CreateAssetMenu(fileName = "New ItemHealEffect", menuName = "New ItemEffect/itemHungryEffect")]
    public class HealHungryEffect : ItemEffect
    {
        public int hungryHealValue;
    
        public override void Effect(ConsumerableItem item, PlayerStatus stat)
        {
            stat.Hungry += hungryHealValue;
            if(stat.Hungry < stat.minHungry) stat.Hungry = stat.minHungry;
            else if(stat.Hungry > stat.maxHungry) stat.Hungry = stat.maxHungry;
            stat.hungryAlter.Invoke(stat.Hungry, stat.maxHungry);
        }
    }
}
