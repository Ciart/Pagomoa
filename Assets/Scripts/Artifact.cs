using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
    public List<ShopItemData> EquipArtifacts = new List<ShopItemData>();
    public InventoryDB inventoryDB;
    public Inventory inventory;
    public GameObject slots;
    public void AddEquipArtifact(ShopItemData data)
    {
        if(transform.GetChild(EquipArtifacts.Count).GetChild(0).GetComponent<Image>().sprite == null)
            slots.transform.GetChild(EquipArtifacts.Count).GetComponent<Slot>().SetArtifactSlot(data);
        EquipArtifacts.Add(data);
    }
    public void RemoveEquipArtifact(ShopItemData data)
    {
        EquipArtifacts.Remove(data);
        UpdateArtifactSlot();
        ReturnInventory(data);
        inventory.UpdateEquipmentSlot();
    }
    public void UpdateArtifactSlot()
    {
        for(int i = 0; i < slots.transform.childCount; i++)
            slots.transform.GetChild(i).GetComponent<Slot>().SetNullArtifactSlot();
        for (int i = 0; i < EquipArtifacts.Count; i++)
            slots.transform.GetChild(i).GetComponent<Slot>().SetArtifactSlot(EquipArtifacts[i]);
    }
    public void ReturnInventory(ShopItemData data)
    {
        inventoryDB.ShopItemsData.Add(data);
    }
}
