using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryDB : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>(new InventoryItem[30]);
    public int Gold;
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

        //if (GameObject.Find("Inventory(Clone)") != null)
        //    changeInventory.AddListener(EtcInventory.Instance.ResetSlot);

        SaveManager.Instance.LoadItem();
    }
    public void Add(Item data, int count = 1) // Item data
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
        var quickSlot = QuickSlotItemDB.instance.quickSlots.Find(quickSlot => quickSlot.inventoryItem.item == data);
        var achieveItem = Achievements.Instance.AchieveMinerals.Find(achieveItem => achieveItem.item == data);
        if (inventoryItem != null)
        {
            inventoryItem.count += count;

            if (quickSlot != null)
                quickSlot.itemCount.text = inventoryItem.count.ToString();

            else if (quickSlot == null)
                return;
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
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
        if (EtcInventory.Instance)
            EtcInventory.Instance.ResetSlot();
    }
    public void Remove(Item data)
    {
        Use(data);
        Gold += data.itemPrice;
        Sell.Instance.Gold.GetComponent<Text>().text = Gold.ToString();
        buy.gold.GetComponent<Text>().text = Gold.ToString();
        //EtcInventory.Instance.ResetSlot();
        //Sell.Instance.ResetSlot();
        //changeInventory.Invoke();
    }
    public void DeleteItem(Item data)
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
        if (inventoryItem != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == inventoryItem)
                {
                    items.RemoveAt(i);
                    items.Insert(i, new InventoryItem(null, 0));
                }
            }
        }
    }
    public void Use(Item data)
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
        if (inventoryItem != null)
        {
            if (inventoryItem.count > 1)
                inventoryItem.count--;
            else if (inventoryItem.count == 1 || inventoryItem.count == 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == inventoryItem)
                    {
                        items.RemoveAt(i);
                        items.Insert(i, new InventoryItem(null, 0));
                    }
                }
            }
        }
    }
    public void Equip(Item data)
    {
        var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
        if (inventoryItem != null && inventoryItem.count == 0)
        {
            Use(data);
        }
        EtcInventory.Instance.ResetSlot();
        //Sell.Instance.ResetSlot();
        //changeInventory.Invoke();
    }
    
}
