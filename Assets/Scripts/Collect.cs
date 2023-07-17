using System.Collections;
using System.Collections.Generic;
using Maps;
using UnityEngine;
using UnityEngine.Events;

public class Collect : MonoBehaviour
{
    public Inventory inventory;
    public ShopInventory shopInventory;
    public InventoryDB inventoryDB;
    public UnityEvent OnCollectEvent;
    private void Awake()
    {
        if (OnCollectEvent == null)
            OnCollectEvent = new UnityEvent();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<MineralEntity>()) return;
        Mineral mineral = collision.gameObject.GetComponent<MineralEntity>().data;

        Debug.Log($"{mineral.tier}티어 광물 \"{mineral.mineralName}\" 를 수집했습니다!");
        OnCollectEvent.Invoke();
        if (inventoryDB)
        {
            inventoryDB.Add(mineral);
            inventory.UpdateEtcSlot();
        }
        if(shopInventory)
            shopInventory.UpdateEtcSlot();
        //inventory.Add(mineral);
        //shopInventory.Add(mineral);
        Destroy(collision.gameObject);
    }
}