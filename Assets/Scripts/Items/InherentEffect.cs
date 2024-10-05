using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    public abstract class InherentEffect : ScriptableObject
    {
        public abstract void Effect(InherentItem item, PlayerStatus stat);
    }
}
