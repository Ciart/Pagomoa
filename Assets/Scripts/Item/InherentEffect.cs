using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InherentEffect : ScriptableObject
{
    public abstract void Effect(InherentItem item, Status stat);
}
