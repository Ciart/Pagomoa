using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public abstract void Effect(ConsumerableItem item, Status stat);
}