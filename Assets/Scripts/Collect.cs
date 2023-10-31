using System;
using Quest;
using Worlds;
using UnityEngine;
using UnityEngine.Events;

public class Collect : MonoBehaviour
{
    //public Inventory inventory;
    //public ShopInventory shopInventory;
    public InventoryDB inventoryDB;
    public EtcInventory etcInventory;
    public UnityEvent OnCollectEvent;
    private void Awake()
    {
        if (OnCollectEvent == null)
            OnCollectEvent = new UnityEvent();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("ItemEntityTrigger"))
        {
            return;
        }

        var itemEntity = other.transform.parent.GetComponent<ItemEntity>();
        var item = itemEntity.Item;

        if (item is null)
        {
            return;
        }
        
        OnCollectEvent.Invoke();
        inventoryDB.Add(item);
        GameLogger.Instance.LogObject(GameLogger.LoggingGeneral.Mineral);
        Destroy(itemEntity.gameObject);
        
        if (item.name == "PowerGemEarth")
        {
            GameManager.instance.hasPowerGemEarth = true;
        }
    }
}