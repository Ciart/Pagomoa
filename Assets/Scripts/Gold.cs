using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    public InventoryDB inventoryDB;
    public void SetGold()
    {
        GetComponent<Text>().text = inventoryDB.Gold.ToString(); 
    }
}
