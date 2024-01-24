using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [CreateAssetMenu(fileName = "New ItemUFORemoteInherentEffect", menuName = "New ItemInherentEffect/itemUFORemoteInherentEffect")]
    [Serializable]
    public class UFORemoteInherentEffect : InherentEffect
    {
        public override void Effect(InherentItem item, PlayerStatus stat)
        {
            WorldManager.instance.MoveUfoBase();
        }
    }
}
