using System.Collections;
using System.Collections.Generic;
using Entities.Players;
using UnityEngine;

public abstract class InherentEffect : ScriptableObject
{
    public abstract void Effect(InherentItem item, PlayerStatus stat);
}
