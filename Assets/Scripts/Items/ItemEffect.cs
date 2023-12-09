using System.Collections;
using System.Collections.Generic;
using Entities.Players;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public abstract void Effect(ConsumerableItem item, PlayerStatus stat);
}
