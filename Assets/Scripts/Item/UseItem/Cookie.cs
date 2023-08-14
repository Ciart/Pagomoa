using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/useitem/Cookie")]
public class Cookie : Item
{
    public override void Active(Status status = null)
    {
        if (status == null) return;
        status.hungry += 15;
        status.hungryAlter.Invoke(status.hungry, status.maxHungry);
    }
}
