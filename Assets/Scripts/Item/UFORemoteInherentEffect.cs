using Player;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Worlds;

[CreateAssetMenu(fileName = "New ItemUFORemoteInherentEffect", menuName = "New ItemInherentEffect/itemUFORemoteInherentEffect")]
[Serializable]
public class UFORemoteInherentEffect : ItemInherentEffect
{
    public override void InherentEffect(InherentItem item, Status stat)
    {
        WorldManager.instance.MoveUfoBase();
    }
}
