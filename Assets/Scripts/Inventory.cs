using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<MineralData> Minerals = new List<MineralData>();
    public GameObject slots;
    public GameObject slot;
    public void Add(MineralData data)
    {
        for (int n = 0; n < Minerals.Count; n++)
        {
            if (Minerals[n] == data)
            {
                slots.transform.GetChild(n+1).GetComponent<Slot>().UpdateCount(1);
                return;
            }
        }
        Minerals.Add(data);
        GameObject item = Instantiate(slot, slots.transform);
        item.SetActive(true);
        item.GetComponent<Slot>().UpdateSlot(Minerals[Minerals.Count-1].sprite, 1);
    }
    public void UpdateInventory()
    {
        Debug.Log(Minerals);
    }
}

