using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotItemDB : MonoBehaviour
{
    static public QuickSlotItemDB instance;

    [SerializeField] public List<InventoryItem> QuickSlotItems = new List<InventoryItem>();

    private void Start()
    {
        instance = this;
    }
}
