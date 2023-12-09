using System;
using Worlds;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
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
        InventoryDB.Instance.Add(item);
        Destroy(itemEntity.gameObject);

        if (item.name == "PowerGemEarth")
        {
            GameManager.instance.hasPowerGemEarth = true;
        }
    }
}