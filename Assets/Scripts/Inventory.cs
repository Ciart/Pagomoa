using System.Collections;
using System.Collections.Generic;
using Maps;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Mineral> Minerals = new List<Mineral>();
    public GameObject slots;
    public GameObject slot;
    public void Add(Mineral data)
    {
        for (int n = 0; n < Minerals.Count; n++)
        {
            if (Minerals[n] == data)
            {
                slots.transform.GetChild(n).GetComponent<Slot>().UpdateCount(1);
                return;
            }
        }
        Minerals.Add(data);
        Instantiate(slot, slots.transform);
        slots.transform.GetChild(Minerals.Count - 1).GetComponent<Slot>().UpdateSlot(Minerals[Minerals.Count-1].sprite, 1);
    }
}

