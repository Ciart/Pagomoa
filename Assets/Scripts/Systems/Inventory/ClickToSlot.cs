using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ClickToSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RightClickMenu _rightClickMenu;
        [SerializeField] private StoneCount _stoneCount;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                InventoryUI.Instance.choiceSlot = this.gameObject.GetComponent<InventorySlotUI>();
                var itemtype = InventoryUI.Instance.choiceSlot.inventoryItem.item.itemType;
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
            var inventory = InventoryUI.Instance;

            if (inventory.choiceSlot.inventoryItem.item == null || inventory.choiceSlot.inventoryItem == null)
                return;

            if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
                EquipItem();

            else if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
            {
                inventory.choiceSlot.inventoryItem.count -= 1;
                UseItem();
                if (inventory.choiceSlot.inventoryItem.count == 0)
                {
                    GameManager.player.inventoryDB.SellItem(inventory.choiceSlot.inventoryItem.item);
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
            if (GameManager.player.inventoryDB.artifactItems.Length < 4 && InventoryUI.Instance.choiceSlot.inventoryItem != null)
            {
                InventoryUI.Instance.DeleteSlot();
                GameManager.player.inventoryDB.AddArtifactData(InventoryUI.Instance.choiceSlot.inventoryItem.item);
                GameManager.player.inventoryDB.Equip(InventoryUI.Instance.choiceSlot.inventoryItem.item);
                InventoryUI.Instance.UpdateSlot();
                InventoryUI.Instance.SetArtifactSlots();
            }
            else
                return;
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatMineral()
        {
            var inventory = InventoryUI.Instance;
            if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Mineral)
            {
                if (inventory.choiceSlot.inventoryItem.count > 1)
                    inventory.choiceSlot.inventoryItem.count -= 1;
                else if(inventory.choiceSlot.inventoryItem.count == 1)
                    GameManager.player.inventoryDB.RemoveItemData(inventory.choiceSlot.inventoryItem.item);
                InventoryUI.Instance.DeleteSlot();
                InventoryUI.Instance.UpdateSlot();
                _stoneCount.UpCount(1);
            }
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatTenMineral()
        {
            var inventory = InventoryUI.Instance;
            if (inventory.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Mineral)
            {
                if (inventory.choiceSlot.inventoryItem.count > 10)
                    inventory.choiceSlot.inventoryItem.count -= 10;
                else if (inventory.choiceSlot.inventoryItem.count == 10)
                    GameManager.player.inventoryDB.RemoveItemData(inventory.choiceSlot.inventoryItem.item);
                
                InventoryUI.Instance.DeleteSlot();
                InventoryUI.Instance.UpdateSlot();
                _stoneCount.UpCount(10);
            }
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatAllMineral()
        {
            var inventory = InventoryUI.Instance;
            int count = inventory.choiceSlot.inventoryItem.count;
            inventory.choiceSlot.inventoryItem.count -= count;
            _stoneCount.UpCount(count);
            GameManager.player.inventoryDB.RemoveItemData(inventory.choiceSlot.inventoryItem.item);
            InventoryUI.Instance.DeleteSlot();
            InventoryUI.Instance.UpdateSlot();
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void UseItem()
        {
            PlayerStatus playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            InventoryUI.Instance.choiceSlot.inventoryItem.item.Active(playerStatus);
            GameManager.player.inventoryDB.DecreaseItemCount(InventoryUI.Instance.choiceSlot.inventoryItem.item);
            InventoryUI.Instance.DeleteSlot();
            InventoryUI.Instance.ResetSlot();
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void AbandonItem()
        {
            GameManager.player.inventoryDB.RemoveItemData(InventoryUI.Instance.choiceSlot.inventoryItem.item);
            InventoryUI.Instance.ResetSlot();
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
    }
}
