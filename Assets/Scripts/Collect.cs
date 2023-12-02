using System;
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
        inventoryDB.Add(mineral);
        Destroy(collision.gameObject);

        //
        Logger.Instance.LoggingObject(this);
        //
        
        if (mineral.name == "PowerGemEarth")
        {
            GameManager.instance.hasPowerGemEarth = true;
        }
    }
}