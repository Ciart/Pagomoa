using System.Collections.Generic;
using Inventory;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public InventoryItem inventoryItem;
    [SerializeField] private EquipUI equipUI;
    [SerializeField] private SellCountUI sellCountUI;
    [SerializeField] private BuyCountUI buyCountUI;
    [SerializeField] private BuyNoCountUI buyNoCountUI;
    [SerializeField] public Image image;
    [SerializeField] public Text text;
    [SerializeField] public int id;

    public void SellCheck()
    {
        EtcInventory.Instance.choiceSlot = this;

        var Inventory = EtcInventory.Instance.choiceSlot.inventoryItem.item;
        if (Inventory.itemType == Item.ItemType.Use ||
            Inventory.itemType == Item.ItemType.Mineral)
        {
            sellCountUI.OnUI();
            sellCountUI.ItemImage(Inventory.itemImage);
        }
    }
    public void ClickSlot()
    {
        for (int i = 0; i < sellCountUI.count; i++)
        {
            EtcInventory.Instance.DeleteSlot();
            InventoryDB.Instance.Remove(EtcInventory.Instance.choiceSlot.inventoryItem.item);
        }
        sellCountUI.count = 0;
        sellCountUI.price = 0;
        sellCountUI.itemCount.text = sellCountUI.count.ToString();
        sellCountUI.totalPrice.text = sellCountUI.price.ToString();
        sellCountUI.OffUI();
    }
    public void BuyCheck()
    {
        Buy.Instance.choiceSlot = this;

        var Shop = Buy.Instance.choiceSlot.inventoryItem.item;
        if (Shop.itemType == Item.ItemType.Use)
        {
            buyCountUI.OnUI();
            buyCountUI.ItemImage(Shop.itemImage);
        }
        else if (Shop.itemType == Item.ItemType.Equipment || Shop.itemType == Item.ItemType.Inherent)
        {
            buyNoCountUI.OnUI();
            buyNoCountUI.ItemImage(Shop.itemImage);
        }
    }
    public void BuySlot()
    {
        var Shop = Buy.Instance.choiceSlot.inventoryItem;
        if (Shop.item.itemType == Item.ItemType.Use)
        {
            if (InventoryDB.Instance.Gold >= Shop.item.itemPrice * buyCountUI.count)
            {
                Shop.count -= buyCountUI.count;
                InventoryDB.Instance.Add(Shop.item, buyCountUI.count);
                AuctionDB.Instance.Remove(Shop.item);
                Buy.Instance.UpdateSlot();
                buyCountUI.OffUI();
            }
            else
                return;

            if (Shop.count == 0)
            {
                AuctionDB.Instance.Remove(Shop.item);
                Buy.Instance.DestroySlot();
                Buy.Instance.AuctionSlot();
                Buy.Instance.UpdateSlot();
                buyCountUI.OffUI();
            }
        }

        else if (Shop.item.itemType == Item.ItemType.Equipment || Shop.item.itemType == Item.ItemType.Inherent)
        {
            if (InventoryDB.Instance.Gold >= Shop.item.itemPrice)
            {
                InventoryDB.Instance.Add(Shop.item, Shop.count);
                AuctionDB.Instance.Remove(Shop.item);
                Buy.Instance.DestroySlot();
                Buy.Instance.AuctionSlot();
                Buy.Instance.UpdateSlot();
                buyNoCountUI.OffUI();
            }
            else
                return;
        }
    }
    public void EquipCheck()
    {
        EtcInventory.Instance.choiceSlot = this;

        var Inventory = EtcInventory.Instance;
        if (Inventory.choiceSlot.inventoryItem == null) { Debug.LogWarning("no Choiced inventoryItem 그러니 고쳐라 승연킴"); return; }

        if (Inventory.choiceSlot.inventoryItem.item == null)
            return;

        if (Inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
            equipUI.OnUI();

        else if (Inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            Inventory.choiceSlot.inventoryItem.count -= 1;
            UseItem();
            if (Inventory.choiceSlot.inventoryItem.count == 0)
            {
                InventoryDB.Instance.Remove(Inventory.choiceSlot.inventoryItem.item);
                Inventory.ResetSlot();
            }
            Inventory.DeleteSlot();
            Inventory.UpdateSlot();
        }
        else
            return;
    }
    public void EquipItem()
    {
        if (ArtifactSlotDB.Instance.Artifact.Count < 4 && inventoryItem != null)
        {
            EtcInventory.Instance.DeleteSlot();
            ArtifactSlotDB.Instance.Artifact.Add(EtcInventory.Instance.choiceSlot.inventoryItem);
            InventoryDB.Instance.Equip(EtcInventory.Instance.choiceSlot.inventoryItem.item);
            EtcInventory.Instance.UpdateSlot();
            ArtifactContent.Instance.ResetSlot();
        }
        else
            return;
        equipUI.OffUI();
    }
    public void ReleaseItem()
    {
        InventoryDB.Instance.Add(inventoryItem.item, 0);
        EtcInventory.Instance.ResetSlot();
        ArtifactSlotDB.Instance.Remove(inventoryItem.item);
        ArtifactContent.Instance.DeleteSlot();
        ArtifactContent.Instance.ResetSlot();
    }
    public void SetUI(Sprite s, string m)
    {
        image.sprite = s;
        text.text = m;
    }
    public void SetUI(Sprite s)
    {
        image.sprite = s;
    }
    public void Return()
    {
        equipUI.OffUI();
        return;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Swap(InventoryDB.Instance.items, this.id, eventData.pointerPress.GetComponent<Slot>().id);
        Swap(this.inventoryItem, eventData.pointerPress.GetComponent<Slot>().inventoryItem);
        InventoryDB.Instance.changeInventory.Invoke();
    }
    public void Swap(List<InventoryItem> list, int i, int j)
    {
        (list[i], list[j]) = (list[j], list[i]);
    }
    public void Swap(InventoryItem item1, InventoryItem item2)
    {
        (item1, item2) = (item2, item1);
    }
    public void UseItem()
    {
        Status playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<Status>();
        this.inventoryItem.item.Active(playerStatus);
    }
}
