using Inventory;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public InventoryItem inventoryItem;
    [SerializeField] private EquipUI equipUI;
    [SerializeField] private SellCountUI sellCountUI;
    [SerializeField] public BuyCountUI buyCountUI;
    [SerializeField] public BuyNoCountUI buyNoCountUI;
    [SerializeField] public Image image;
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public int id;

    public void SellCheck()
    {
        Sell.Instance.choiceSlot = this;

        var Inventory = Sell.Instance.choiceSlot.inventoryItem.item;
        if (Inventory == null)
            return;

        if (Inventory.itemType == Item.ItemType.Use ||
            Inventory.itemType == Item.ItemType.Mineral)
        {
            sellCountUI.OnUI();
            ShopChat.Instance.SellPriceToChat(Inventory.itemPrice);
        }
    }

    public void ClickSlot()
    {
        for (int i = 0; i < sellCountUI.count; i++)
        {
            //EtcInventory.Instance.DeleteSlot();
            if (Sell.Instance.choiceSlot.inventoryItem.count > 1)
            {
                InventoryDB.Instance.Remove(Sell.Instance.choiceSlot.inventoryItem.item);
                QuickSlotItemDB.instance.SetCount(Sell.Instance.choiceSlot.inventoryItem.item);
            }
            else if (Sell.Instance.choiceSlot.inventoryItem.count == 1)
            {
                InventoryDB.Instance.Remove(Sell.Instance.choiceSlot.inventoryItem.item);
                QuickSlotItemDB.instance.CleanSlot(Sell.Instance.choiceSlot.inventoryItem.item);
            }
            Sell.Instance.DeleteSlot();
            Sell.Instance.ResetSlot();
        }
        sellCountUI.count = 0;
        sellCountUI.itemCount.text = sellCountUI.count.ToString();
        sellCountUI.OffUI();
    }
    public void ReleaseItem()
    {
        EtcInventory.Instance.choiceSlot = this;
        var inventory = EtcInventory.Instance;

        if (inventory.choiceSlot.inventoryItem == null || inventory.choiceSlot.inventoryItem.item == null)
            return;

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
