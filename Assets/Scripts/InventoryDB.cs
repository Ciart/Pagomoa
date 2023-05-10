using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDB : MonoBehaviour
{
    public Dictionary<MineralData, int> MineralsData = new Dictionary<MineralData, int>();
    public void Add(MineralData data)
    {
        Debug.Log("È£ÃâµÇÀÝ¾Æ~");
        if (MineralsData.ContainsKey(data))
        {
            MineralsData[data] += 1;
            return;
        }
        MineralsData.Add(data, 1);
    }
    public void Remove(MineralData data)
    {
        if (MineralsData[data] > 1)
        {
            if (MineralsData.ContainsKey(data))
            {
                MineralsData[data] -= 1;
                return;
            }
        }
        else if (MineralsData[data] == 1)
        {
            MineralsData.Remove(data);
        }
    }
}
