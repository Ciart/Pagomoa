using Inventory;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RightClickMenu _rightClickMenu;
    [SerializeField] private EquipUI _equipUI;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            EtcInventory.Instance.choiceSlot = this.gameObject.GetComponent<Slot>();
            var itemtype = EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType;
            Vector3 mouseposition = new Vector3(eventData.position.x + 5, eventData.position.y);
            _rightClickMenu.SetUI();
            _rightClickMenu.gameObject.transform.position = mouseposition;

            if (itemtype == Item.ItemType.Equipment)
            {
                _rightClickMenu.EquipmentMenu();
            }
            else if (itemtype == Item.ItemType.Mineral)
            {
                _rightClickMenu.MineralMenu();
            }
            else if (itemtype == Item.ItemType.Use)
            {
                _rightClickMenu.UseMenu();
            }
            else if (itemtype == Item.ItemType.Inherent)
            {
                _rightClickMenu.InherentMenu();
            }
        }
        else
            return;
    }
    public void EquipCheck()
    {
        //EtcInventory.Instance.choiceSlot = this;

        var inventory = EtcInventory.Instance;

        if (inventory.choiceSlot.inventoryItem.item == null || inventory.choiceSlot.inventoryItem == null)
            return;

        if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
            _equipUI.OnUI();

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
        if (ArtifactSlotDB.Instance.Artifact.Count < 4 && EtcInventory.Instance.choiceSlot.inventoryItem != null)
        {
            EtcInventory.Instance.DeleteSlot();
            ArtifactSlotDB.Instance.Artifact.Add(EtcInventory.Instance.choiceSlot.inventoryItem);
            InventoryDB.Instance.Equip(EtcInventory.Instance.choiceSlot.inventoryItem.item);
            EtcInventory.Instance.UpdateSlot();
            ArtifactContent.Instance.ResetSlot();
        }
        else
            return;
        _equipUI.OffUI();
        _rightClickMenu.SetUI();
        _rightClickMenu.DeleteMenu();
    }
    public void EatMineral()
    {
        var inventory = EtcInventory.Instance;
        if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Mineral)
        {
            if (inventory.choiceSlot.inventoryItem.count > 1)
                inventory.choiceSlot.inventoryItem.count -= 1;
            else if(inventory.choiceSlot.inventoryItem.count == 1)
                InventoryDB.Instance.DeleteItem(inventory.choiceSlot.inventoryItem.item);
            EtcInventory.Instance.DeleteSlot();
            EtcInventory.Instance.UpdateSlot();
        }
        _rightClickMenu.SetUI();
        _rightClickMenu.DeleteMenu();
    }
    public void EatTenMineral()
    {
        var inventory = EtcInventory.Instance;
        if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Mineral)
        {
            if (inventory.choiceSlot.inventoryItem.count > 10)
                inventory.choiceSlot.inventoryItem.count -= 10;
            else if (inventory.choiceSlot.inventoryItem.count == 10)
                InventoryDB.Instance.DeleteItem(inventory.choiceSlot.inventoryItem.item);
            else if (inventory.choiceSlot.inventoryItem.count < 10)
                Debug.Log("10개보다 적음");
            EtcInventory.Instance.DeleteSlot();
            EtcInventory.Instance.UpdateSlot();
        }
        _rightClickMenu.SetUI();
        _rightClickMenu.DeleteMenu();
    }
    public void UseItem()
    {
        Status playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<Status>();
        EtcInventory.Instance.choiceSlot.inventoryItem.item.Active(playerStatus);
        InventoryDB.Instance.Use(EtcInventory.Instance.choiceSlot.inventoryItem.item);
        EtcInventory.Instance.DeleteSlot();
        EtcInventory.Instance.ResetSlot();
        _rightClickMenu.SetUI();
        _rightClickMenu.DeleteMenu();
    }
    public void AbandonItem()
    {
        InventoryDB.Instance.DeleteItem(EtcInventory.Instance.choiceSlot.inventoryItem.item);
        EtcInventory.Instance.ResetSlot();
        _rightClickMenu.SetUI();
        _rightClickMenu.DeleteMenu();
    }
}
