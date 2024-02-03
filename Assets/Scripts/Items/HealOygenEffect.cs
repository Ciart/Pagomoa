using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [CreateAssetMenu(fileName = "New ItemHealEffect", menuName = "New ItemEffect/itemOxygenEffect")]
    public class HealOygenEffect : ItemEffect
    {
        public int oxygenHealValue;
        public override void Effect(ConsumerableItem item, PlayerStatus stat)
        {
            stat.oxygen += oxygenHealValue;
        }
    }
}
