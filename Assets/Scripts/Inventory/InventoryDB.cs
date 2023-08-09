using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryDB : MonoBehaviour
    {
        public List<InventoryItem> items = new List<InventoryItem>();
        public int Gold;
        [SerializeField] private EtcInventory inventory;
        [SerializeField] private Buy buy;

        public UnityEvent makeSlots;
        public UnityEvent changeInventory;
        public UnityEvent marketCondition;

        private static InventoryDB instance;
        public static InventoryDB Instance
        {
            get
            {
                if (!instance)
                {
                    instance = GameObject.FindObjectOfType(typeof(InventoryDB)) as InventoryDB;
                }
                return instance;
            }
        }
        private void Awake()
        {
            makeSlots.Invoke();
        }
        public void Add(Itembefore data, int count = 1) // Item data
        {
            var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
            var achieveItem = Achievements.Instance.AchieveMinerals.Find(achieveItem => achieveItem.item == data);
            if (inventoryItem != null)
            {
                inventoryItem.count += count;
            }
            else
            {
                items.Add(new InventoryItem(data, count));
                if (data.itemType == Itembefore.ItemType.Mineral)
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
        public void Remove(Itembefore data)
        {
            var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
            if (inventoryItem != null)
            {
                if (inventoryItem.count > 1)
                    inventoryItem.count--;
                else if (inventoryItem.count == 1 || inventoryItem.count == 0)
                    items.Remove(inventoryItem);
            }
            Gold += data.itemPrice;
            EtcInventory.Instance.gold.GetComponent<Text>().text = Gold.ToString();
            buy.gold.GetComponent<Text>().text = Gold.ToString();
            //Buy.Instance.gold.GetComponent<Text>().text = Gold.ToString();
            changeInventory.Invoke();
        }
        public void Equip(Itembefore data)
        {
            var inventoryItem = items.Find(inventoryItem => inventoryItem.item == data);
            if (inventoryItem != null && inventoryItem.count == 0)
            {
                items.Remove(inventoryItem);
            }
            changeInventory.Invoke();
        }
    }
