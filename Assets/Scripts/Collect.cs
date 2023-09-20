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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<ItemEntity>()) return;
        
        var mineral = collision.gameObject.GetComponent<ItemEntity>().Item;

        Debug.Log($"\"{mineral.itemName}\" 를 수집했습니다!");
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