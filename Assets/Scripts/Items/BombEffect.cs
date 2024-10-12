using System;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [Serializable]
    public class BombEffect : NewItemEffect
    {
        public override void Effect()
        {
            Debug.Log("Spawn Bomb!!!");
        }
    }
}
