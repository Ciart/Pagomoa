using System;
using System.Collections;
using System.Collections.Generic;
using Maps;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class InventoryDB : MonoBehaviour
{
    public Dictionary<Mineral, int> MineralsData = new Dictionary<Mineral, int>();
    public Dictionary<Item, int> ConsumptionItemsData = new Dictionary<Item, int>();
    public List<Item> MineralData = new List<Item>();
    public List<Item> ShopItemsData = new List<Item>();
    public int Gold;
    public Count mineralCount;
    [SerializeField] private EtcInventory inventory;

    public void AddMineral(Mineral data)
    {
        if (data.item.itemName == "CopperItem" && MineralData.Contains(data.item))
        {
            mineralCount.copperCount.Count++;
            inventory.NotNull(data);
            return;
        }
        else if (data.item.itemName == "IronItem" && MineralData.Contains(data.item))
        {
            mineralCount.ironCount.Count++;
            inventory.NotNull(data);
            return;
        }

        if (data.item.itemName == "CopperItem" && !MineralData.Contains(data.item))
        {
            MineralData.Add(data.item);
            mineralCount.copperCount.Count++;
            inventory.InputSlot(data);
        }
        if (data.item.itemName == "IronItem" && !MineralData.Contains(data.item))
        {
            MineralData.Add(data.item);
            mineralCount.ironCount.Count++;
            inventory.InputSlot(data);
        }
    }
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
    public void AddEquipmentItemData(Item data)
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
    public void AddConsumptionItemData(Item data)
    {
        if (ConsumptionItemsData.ContainsKey(data))
        {
            ConsumptionItemsData[data] += 1;
            return;
        }
        ConsumptionItemsData.Add(data, 1);
    }
    public void RemoveConsumptionItemData(Item data)
    {
        if (ConsumptionItemsData[data] > 1)
        {
            if (ConsumptionItemsData.ContainsKey(data))
            {
                ConsumptionItemsData[data] -= 1;
                Gold += data.itemPrice;
                Debug.Log(Gold);
                return;
            }
        }
        else if (ConsumptionItemsData[data] == 1)
        {
            ConsumptionItemsData.Remove(data);
            Gold += data.itemPrice;
        }
    }
    public void RemoveArtifactItemData(Item data)
    {
        ShopItemsData.Remove(data);
    }
}
