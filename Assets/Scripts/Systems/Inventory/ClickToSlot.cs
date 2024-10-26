using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
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
                var choiceSlot = GameManager.player.inventory.items[InventoryUI.Instance.choiceSlot.id];
                var itemType = choiceSlot.item.type;
                Vector3 mouseposition = new Vector3(eventData.position.x + 5, eventData.position.y);
                _rightClickMenu.SetUI();
                _rightClickMenu.gameObject.transform.position = mouseposition;

                if (itemType == ItemType.Equipment)
                {
                    _rightClickMenu.EquipmentMenu();
                }
                else if (itemType == ItemType.Mineral)
                {
                    _rightClickMenu.MineralMenu(choiceSlot.count);
                }
                else if (itemType == ItemType.Use)
                {
                    _rightClickMenu.UseMenu();
                }
                else if (itemType == ItemType.Inherent)
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

            if (GameManager.player.inventory.items[inventory.choiceSlot.id].item == null)
                return;

            if (GameManager.player.inventory.items[inventory.choiceSlot.id].item.type == ItemType.Equipment)
                EquipItem();

            else if (GameManager.player.inventory.items[inventory.choiceSlot.id].item.type == ItemType.Use)
            {
                GameManager.player.inventory.items[inventory.choiceSlot.id].count -= 1;
                UseItem();
                if (GameManager.player.inventory.items[inventory.choiceSlot.id].count == 0)
                {
                    GameManager.player.inventory.SellItem(GameManager.player.inventory.items[inventory.choiceSlot.id].item);
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
            const int mineralCount = 1;
            
            if (GameManager.player.inventory.items[inventory.choiceSlot.id].item.itemType == Item.ItemType.Mineral)
            {
                if (GameManager.player.inventory.items[inventory.choiceSlot.id].count > mineralCount)
                {
                    GameManager.player.inventory.items[inventory.choiceSlot.id].count -= mineralCount;
                    EventManager.Notify(new ItemUsedEvent(GameManager.player.inventory.items[inventory.choiceSlot.id].item, mineralCount));
                }
                else if(GameManager.player.inventory.items[inventory.choiceSlot.id].count == mineralCount)
                    GameManager.player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
                inventory.ResetSlots();
                inventory.UpdateSlots();
                _stoneCount.UpCount(mineralCount);
            }
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatTenMineral()
        {
            var inventory = InventoryUI.Instance;
            const int mineralCount = 10;
            
            if (GameManager.player.inventory.items[inventory.choiceSlot.id].item.itemType == Item.ItemType.Mineral)
            {
                if (GameManager.player.inventory.items[inventory.choiceSlot.id].count > mineralCount)
                {
                    GameManager.player.inventory.items[inventory.choiceSlot.id].count -= mineralCount;
                    EventManager.Notify(new ItemUsedEvent(GameManager.player.inventory.items[inventory.choiceSlot.id].item, mineralCount));
                }
                else if (GameManager.player.inventory.items[inventory.choiceSlot.id].count == mineralCount)
                    GameManager.player.inventory.RemoveItemData(GameManager.player.inventory.items[inventory.choiceSlot.id].item);
                
                inventory.ResetSlots();
                inventory.UpdateSlots();
                _stoneCount.UpCount(mineralCount);
            }
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatAllMineral()
        {
            var inventory = InventoryUI.Instance;
            int count = GameManager.player.inventory.items[inventory.choiceSlot.id].count;
            GameManager.player.inventory.items[inventory.choiceSlot.id].count -= count;
            EventManager.Notify(new ItemUsedEvent(GameManager.player.inventory.items[inventory.choiceSlot.id].item, count));
            _stoneCount.UpCount(count);
            GameManager.player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
            inventory.ResetSlots();
            inventory.UpdateSlots();
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void UseItem()
        {
            var chosenItem = GameManager.player.inventory.items[InventoryUI.Instance.choiceSlot.id].item;
            
            PlayerStatus playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            GameManager.player.inventory.items[InventoryUI.Instance.choiceSlot.id].item.Active(playerStatus);
            GameManager.player.inventory.DecreaseItemCount(chosenItem);
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
