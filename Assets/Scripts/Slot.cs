using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Sellbtn sellbutton;
    public InventoryDB inventoryDB;
    public MineralData mineralData;
    public Inventory inventory;
    public ShopInventory shopInventory;
    
    public void SetSlot(MineralData data, int count)
    {
        mineralData = data;
        transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
        transform.GetComponentInChildren<Text>().text = count.ToString();
    }
    public void Choice()
    {
        if (sellbutton.ButtonOnClick)
        {
            inventoryDB.Remove(mineralData);
            inventory.UpdateSlot();
            shopInventory.UpdateSlot();
        }
    }
}

