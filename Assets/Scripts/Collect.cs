using System.Collections;
using System.Collections.Generic;
using Worlds;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.tvOS;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<ItemEntity>()) return;
        
<<<<<<< HEAD
        if (!collision.gameObject.GetComponent<ItemEntity>()) return;
        var mineral = collision.gameObject.GetComponent<ItemEntity>().item;
=======
        var mineral = collision.gameObject.GetComponent<ItemEntity>().Item;
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85

        Debug.Log($"\"{mineral.itemName}\" 를 수집했습니다!");
        OnCollectEvent.Invoke();
        inventoryDB.Add(mineral);
        Destroy(collision.gameObject);

<<<<<<< HEAD
    private void CollectInherentItem(Collision2D collision)
    {
        /*if (collision.gameObject.GetComponent<UFORemoteControl>())
        {
            InherentItem inherentItem = collision.gameObject.GetComponent<UFORemoteControl>().inherentItem;
            OnCollectEvent.Invoke();
            inventoryDB.Add(inherentItem);
            Destroy(collision.gameObject);
        }*/
=======
        if (mineral.name == "PowerGemEarth")
        {
            GameManager.instance.hasPowerGemEarth = true;
        }
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    }
}