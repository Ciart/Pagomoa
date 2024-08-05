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
                var itemtype = InventoryUI.Instance.choiceSlot.slot.item.itemType;
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

            if (inventory.choiceSlot.slot.item == null)
                return;

            if (inventory.choiceSlot.slot.item.itemType == Item.ItemType.Equipment)
                EquipItem();

            else if (inventory.choiceSlot.slot.item.itemType == Item.ItemType.Use)
            {
                inventory.choiceSlot.slot.count -= 1;
                UseItem();
                if (inventory.choiceSlot.slot.count == 0)
                {
                    GameManager.player.inventory.SellItem(inventory.choiceSlot.slot.item);
                }
                inventory.ResetSlots();
                inventory.UpdateSlots();
            }
            else
                return;
        }
        public void EquipItem()
        {
            if (GameManager.player.inventory.artifactItems.Length < 4)
            {
                InventoryUI.Instance.ResetSlots();
                GameManager.player.inventory.AddArtifactData(InventoryUI.Instance.choiceSlot.slot.item);
                GameManager.player.inventory.Equip(InventoryUI.Instance.choiceSlot.slot.item);
                InventoryUI.Instance.UpdateSlots();
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
            if (inventory.choiceSlot.slot.item.itemType == Item.ItemType.Mineral)
            {
                if (inventory.choiceSlot.slot.count > 1)
                    inventory.choiceSlot.slot.count -= 1;
                else if(inventory.choiceSlot.slot.count == 1)
                    GameManager.player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
                InventoryUI.Instance.ResetSlots();
                InventoryUI.Instance.UpdateSlots();
                _stoneCount.UpCount(1);
            }
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatTenMineral()
        {
            var inventory = InventoryUI.Instance;
            if (inventory.choiceSlot.slot.item.itemType == Item.ItemType.Mineral)
            {
                if (inventory.choiceSlot.slot.count > 10)
                    inventory.choiceSlot.slot.count -= 10;
                else if (inventory.choiceSlot.slot.count == 10)
                    GameManager.player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
                
                InventoryUI.Instance.ResetSlots();
                InventoryUI.Instance.UpdateSlots();
                _stoneCount.UpCount(10);
            }
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatAllMineral()
        {
            var inventory = InventoryUI.Instance;
            int count = inventory.choiceSlot.slot.count;
            inventory.choiceSlot.slot.count -= count;
            _stoneCount.UpCount(count);
            GameManager.player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
            InventoryUI.Instance.ResetSlots();
            InventoryUI.Instance.UpdateSlots();
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void UseItem()
        {
            PlayerStatus playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            InventoryUI.Instance.choiceSlot.slot.item.Active(playerStatus);
            GameManager.player.inventory.DecreaseItemCount(InventoryUI.Instance.choiceSlot.slot.item);
            InventoryUI.Instance.ResetSlots();
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void AbandonItem()
        {
            GameManager.player.inventory.RemoveItemData(InventoryUI.Instance.choiceSlot.slot.item);
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
    }
}
