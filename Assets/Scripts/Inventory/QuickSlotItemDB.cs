using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotItemDB : MonoBehaviour
{
    static public QuickSlotItemDB instance;

    [SerializeField] public List<InventoryItem> quickSlotItems = new List<InventoryItem>();
    [SerializeField] public List<QuickSlot> quickSlots = new List<QuickSlot>();
    [SerializeField] public QuickSlot selectedSlot;

    private void Start()
    {
        instance = this;
    }
}
