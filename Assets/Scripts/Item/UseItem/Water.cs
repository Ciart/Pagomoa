using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/useitem/Water")]
public class Water : Item
{
    public override void Active(Status status = null)
    {
        if (status == null) return;
        status.oxygen += 15;
    }
}
