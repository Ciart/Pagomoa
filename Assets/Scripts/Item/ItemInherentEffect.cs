using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInherentEffect : ScriptableObject
{
    public abstract void InherentEffect(InherentItem item, Status stat);
}
