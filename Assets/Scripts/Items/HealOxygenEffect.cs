using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [Obsolete("Ciart.Pagomoa.Items.ItemEffectLsit를 확인해주세요.")]
    [CreateAssetMenu(fileName = "New ItemHealEffect", menuName = "New ItemEffect/itemOxygenEffect")]
    public class HealOxygenEffect : ItemEffect
    {
        public int oxygenHealValue;
        public override void Effect(ConsumableItem item, PlayerStatus stat)
        {
            Game.Instance.player.Oxygen += oxygenHealValue;
        }
    }
}
