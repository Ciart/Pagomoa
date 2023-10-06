using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryDB : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>(new InventoryItem[30]);
    public int Gold;
    [SerializeField] private EtcInventory inventory;
    [SerializeField] private Buy buy;

    public UnityEvent makeSlots;
    public UnityEvent changeInventory;
    public UnityEvent marketCondition;

    public static InventoryDB Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        makeSlots.Invoke();
        SaveManager.Instance.LoadItem();
        
    }
    public void Add(Item data, int count = 1) // Item data
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
        var achieveItem = Achievements.Instance.AchieveMinerals.Find(achieveItem => achieveItem.item == data);
        if (inventoryItem != null)
        {
            inventoryItem.count += count;
        }
        else
        {
            for (int i = 0; i < inventory.count; i++)
            {
                if (items[i].item == null)
                {
                    items.Insert(i, new InventoryItem(data, count));
                    items.RemoveAt(i + 1);
                    break;
                }
            }
            if (data.itemType == Item.ItemType.Mineral)
            {
                if (!Achievements.Instance.AchieveMinerals.Contains(achieveItem))
                    Achievements.Instance.AchieveMinerals.Add(new InventoryItem(data, count));
                else
                    return;
                marketCondition.Invoke();
            }
        }
        changeInventory.Invoke();
    }
    public void Remove(Item data)
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
        if (inventoryItem != null)
        {
            if (inventoryItem.count > 1)
                inventoryItem.count--;
            else if (inventoryItem.count == 1 || inventoryItem.count == 0)
            {
                for (int i = 0; i < inventory.count; i++)
                {
                    if (items[i] == inventoryItem)
                    {
                        items.RemoveAt(i);
                        items.Insert(i,new InventoryItem(null, 0));
                    }
                }
            }
        }
        Gold += data.itemPrice;
        EtcInventory.Instance.gold.GetComponent<Text>().text = Gold.ToString();
        buy.gold.GetComponent<Text>().text = Gold.ToString();
        changeInventory.Invoke();
    }
    public void Equip(Item data)
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
        if (inventoryItem != null && inventoryItem.count == 0)
        {
            Remove(data);
        }
        changeInventory.Invoke();
    }
}
