using System.Collections.Generic;
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
        if (EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use ||
            EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Mineral)
        {
            sellCountUI.OnUI();
            sellCountUI.ItemImage(EtcInventory.Instance.choiceSlot.inventoryItem.item.itemImage);
        }
    }
    public void ClickSlot()
    {
        for (int i = 0; i < sellCountUI.count; i++)
        {
            EtcInventory.Instance.DeleteSlot();
            InventoryDB.Instance.Remove(EtcInventory.Instance.choiceSlot.inventoryItem.item);
            EtcInventory.Instance.UpdateSlot();
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
        if (Buy.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            buyCountUI.OnUI();
            buyCountUI.ItemImage(Buy.Instance.choiceSlot.inventoryItem.item.itemImage);
        }
        else if (Buy.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
        {
            buyNoCountUI.OnUI();
            buyNoCountUI.ItemImage(Buy.Instance.choiceSlot.inventoryItem.item.itemImage);
        }
    }
    public void BuySlot()
    {
        if (Buy.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            if (InventoryDB.Instance.Gold >= Buy.Instance.choiceSlot.inventoryItem.item.itemPrice * buyCountUI.count)
            {
                Buy.Instance.choiceSlot.inventoryItem.count -= buyCountUI.count;
                InventoryDB.Instance.Add(Buy.Instance.choiceSlot.inventoryItem.item, buyCountUI.count);
                AuctionDB.Instance.Remove(Buy.Instance.choiceSlot.inventoryItem.item);
                Buy.Instance.UpdateSlot();
                buyCountUI.OffUI();
            }
            else
                return;

            if (Buy.Instance.choiceSlot.inventoryItem.count == 0)
            {
                AuctionDB.Instance.Remove(Buy.Instance.choiceSlot.inventoryItem.item);
                Buy.Instance.DestroySlot();
                Buy.Instance.AuctionSlot();
                Buy.Instance.UpdateSlot();
                buyCountUI.OffUI();
            }
        }

        else if (Buy.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
        {
            if (InventoryDB.Instance.Gold >= Buy.Instance.choiceSlot.inventoryItem.item.itemPrice)
            {
                InventoryDB.Instance.Add(Buy.Instance.choiceSlot.inventoryItem.item, Buy.Instance.choiceSlot.inventoryItem.count);
                AuctionDB.Instance.Remove(Buy.Instance.choiceSlot.inventoryItem.item);
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
        if (EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
            equipUI.OnUI();

        else if (EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            EtcInventory.Instance.choiceSlot.inventoryItem.count -= 1;
            if (EtcInventory.Instance.choiceSlot.inventoryItem.count == 0)
            {
                InventoryDB.Instance.items.Remove(EtcInventory.Instance.choiceSlot.inventoryItem);
                EtcInventory.Instance.ResetSlot();
            }
            EtcInventory.Instance.UpdateSlot();
        }
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
        InventoryDB.Instance.items.Add(inventoryItem);
        EtcInventory.Instance.ResetSlot();
        ArtifactSlotDB.Instance.Remove(inventoryItem.item);
        ArtifactContent.Instance.ResetSlot();
        EtcInventory.Instance.DeleteSlot();
        EtcInventory.Instance.UpdateSlot();
        ArtifactContent.Instance.DeleteSlot();
        ArtifactContent.Instance.UpdateSlot();
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
        EtcInventory.Instance.ResetSlot();
    }
    public void Swap(List<InventoryItem> list, int i, int j)
    {
        (list[i], list[j]) = (list[j], list[i]);
    }
    public void Swap(InventoryItem item1, InventoryItem item2)
    {
        (item1, item2) = (item2, item1);
    }
}
