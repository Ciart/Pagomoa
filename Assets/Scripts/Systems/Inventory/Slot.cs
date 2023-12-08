using Inventory;
using System.Collections.Generic;
using Entities.Players;
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
    
    
    public void EquipCheck()
    {
        EtcInventory.Instance.choiceSlot = this;

        var inventory = EtcInventory.Instance;

        if (inventory.choiceSlot.inventoryItem.item == null || inventory.choiceSlot.inventoryItem == null)
            return;

        if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
            equipUI.OnUI();

        else if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            inventory.choiceSlot.inventoryItem.count -= 1;
            UseItem();
            if (inventory.choiceSlot.inventoryItem.count == 0)
            {
                InventoryDB.Instance.Remove(inventory.choiceSlot.inventoryItem.item);
                inventory.ResetSlot();
            }
            inventory.DeleteSlot();
            inventory.UpdateSlot();
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
    public void UseItem()
    {
        PlayerStatus playerPlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        this.inventoryItem.item.Active(playerPlayerStatus);
    }
}
