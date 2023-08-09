using System;
using UnityEngine;
[Serializable]
public class InventoryItem
{
    public Itembefore item;
    public int count;
    public InventoryItem(Itembefore item, int count)
    {
        this.item = item;
        this.count = count;
    }
}