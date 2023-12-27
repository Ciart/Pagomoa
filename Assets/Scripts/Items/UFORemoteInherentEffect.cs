using System;
using System.Collections;
using Entities.Players;
using Unity.VisualScripting;
using UnityEngine;
using Worlds;

[CreateAssetMenu(fileName = "New ItemUFORemoteInherentEffect", menuName = "New ItemInherentEffect/itemUFORemoteInherentEffect")]
[Serializable]
public class UFORemoteInherentEffect : InherentEffect
{
    public override void Effect(InherentItem item, PlayerStatus stat)
    {
        WorldManager.instance.MoveUfoBase();
    }
}
