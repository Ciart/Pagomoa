using System.Collections;
using System.Collections.Generic;
using Maps;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class InventoryDB : MonoBehaviour
{
    public Dictionary<Mineral, int> MineralsData = new Dictionary<Mineral, int>();
    public Dictionary<ShopConsumptionItemData, int> ConsumptionItemsData = new Dictionary<ShopConsumptionItemData, int>();
    public List<ShopItemData> ShopItemsData = new List<ShopItemData>();
    public int Gold;
    public void Add(Mineral data)
    {
        if (MineralsData.ContainsKey(data))
        {
            MineralsData[data] += 1;
            return;
        }
        MineralsData.Add(data, 1);
    }
    public void Remove(Mineral data)
    {
        if (MineralsData[data] > 1)
        {
            if (MineralsData.ContainsKey(data))
            {
                MineralsData[data] -= 1;
                Gold += data.price;
                Debug.Log(Gold);
                return;
            }
        }
        else if (MineralsData[data] == 1)
        {
            MineralsData.Remove(data);
            Gold += data.price;
        }
    }
    public void AddEquipmentItemData(ShopItemData data)
    {
        if (ShopItemsData.Contains(data))
        {
            return;
        }
        else
        {
            ShopItemsData.Add(data);
        }
    }
    public void AddConsumptionItemData(ShopConsumptionItemData data)
    {
        if (ConsumptionItemsData.ContainsKey(data))
        {
            ConsumptionItemsData[data] += 1;
            return;
        }
        ConsumptionItemsData.Add(data, 1);
    }
    public void RemoveConsumptionItemData(ShopConsumptionItemData data)
    {
        if (ConsumptionItemsData[data] > 1)
        {
            if (ConsumptionItemsData.ContainsKey(data))
            {
                ConsumptionItemsData[data] -= 1;
                Gold += data.price;
                Debug.Log(Gold);
                return;
            }
        }
        else if (ConsumptionItemsData[data] == 1)
        {
            ConsumptionItemsData.Remove(data);
            Gold += data.price;
        }
    }
    public void RemoveArtifactItemData(ShopItemData data)
    {
        ShopItemsData.Remove(data);
    }
}
