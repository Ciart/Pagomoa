using System.Collections;
using System.Collections.Generic;
using Maps;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public SellUI sellBtn;
    public ShopUI buyBtn;
    public InventoryDB inventoryDB;
    public Mineral mineralData;
    public Inventory inventory;
    public ShopInventory shopInventory;
    public ShopItemData shopitemdata;
    public ShopConsumptionItemData consumptionItemData;
    public ShopDB shopDB;
    public BuyUI buyUI;
    public Artifact artifact;
    
    public void SetEtcSlot(Mineral data, int count)
    {
        mineralData = data;
        transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
        transform.GetComponentInChildren<Text>().text = count.ToString();
    }
    public void SetEquipmentSlotShop(ShopItemData data)
    {
        shopitemdata = data;
        transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
    }
    public void SetConsumptionSlotShop(ShopConsumptionItemData data)
    {
        consumptionItemData = data;
        transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
    }
    public void SetConsumptionslot(ShopConsumptionItemData data, int count)
    {
        consumptionItemData = data;
        transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
        transform.GetComponentInChildren<Text>().text = count.ToString() ;
    }
    public void SetArtifactSlot(ShopItemData data)
    {
        shopitemdata = data;
        transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
    }
    public void SetNullArtifactSlot()
    {
        shopitemdata = null;
        transform.GetChild(0).GetComponent<Image>().sprite = null;
    }
    public void Choice()
    {
        if (sellBtn.ButtonOnClick)
        {
            inventoryDB.Remove(mineralData);
            inventory.UpdateEtcSlot();
            shopInventory.UpdateEtcSlot();
        }
    }
    public void ChoiceConsumptionSlotInventory()
    {
        if (sellBtn.ButtonOnClick)
        {
            inventoryDB.RemoveConsumptionItemData(consumptionItemData);
            inventory.UpdateConsumptionSlot();
            shopInventory.UpdateConsumptionSlot();
            //buyUI.UpdateConsumptionSlotShop();
        }
    }
    public void ChoiceEquipmentSlotShop()
    {
        if (buyBtn.ButtonOnClick && inventoryDB.Gold >= shopitemdata.price)
        {
            shopDB.RemoveItem(shopitemdata);
            inventory.UpdateEquipmentSlot();
            buyUI.UpdateEquipmentSlotShop();
        }
        else if(buyBtn.ButtonOnClick && shopitemdata.price >= inventoryDB.Gold)
        {
            return;
        }
    }
    public void ChoiceConsumptionSlotShop()
    {
        if (buyBtn.ButtonOnClick && inventoryDB.Gold >= consumptionItemData.price)
        {
            shopDB.RemoveConsumptionItem(consumptionItemData);
            inventory.UpdateConsumptionSlot();
            shopInventory.UpdateConsumptionSlot();
        }
        else if (buyBtn.ButtonOnClick && consumptionItemData.price >= inventoryDB.Gold)
        {
            return;
        }
    }
    public void ChoiceArtifactSlotInventory()
    {
        inventoryDB.RemoveArtifactItemData(shopitemdata);
        inventory.UpdateEquipmentSlot();
        artifact.AddEquipArtifact(shopitemdata);
    }
    public void ChoiceArtifactSlot()
    {
        artifact.RemoveEquipArtifact(shopitemdata);
    }
}

