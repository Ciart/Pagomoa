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
                var gameManager = GameManager.instance;
                
                InventoryUI.Instance.choiceSlot = this.gameObject.GetComponent<InventorySlotUI>();
                var choiceSlot = gameManager.player.inventory.items[InventoryUI.Instance.choiceSlot.id];
                var itemType = choiceSlot.item.itemType;
                Vector3 mouseposition = new Vector3(eventData.position.x + 5, eventData.position.y);
                _rightClickMenu.SetUI();
                _rightClickMenu.gameObject.transform.position = mouseposition;

                if (itemType == Item.ItemType.Equipment)
                {
                    _rightClickMenu.EquipmentMenu();
                }
                else if (itemType == Item.ItemType.Mineral)
                {
                    _rightClickMenu.MineralMenu(choiceSlot.count);
                }
                else if (itemType == Item.ItemType.Use)
                {
                    _rightClickMenu.UseMenu();
                }
                else if (itemType == Item.ItemType.Inherent)
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
            PlayerController player = GameManager.instance.player;

            if (player.inventory.items[inventory.choiceSlot.id].item == null)
                return;

            if (player.inventory.items[inventory.choiceSlot.id].item.itemType == Item.ItemType.Equipment)
                EquipItem();

            else if (player.inventory.items[inventory.choiceSlot.id].item.itemType == Item.ItemType.Use)
            {
                player.inventory.items[inventory.choiceSlot.id].count -= 1;
                UseItem();
                if (player.inventory.items[inventory.choiceSlot.id].count == 0)
                {
                    player.inventory.SellItem(player.inventory.items[inventory.choiceSlot.id].item);
                }
                inventory.ResetSlots();
                inventory.UpdateSlots();
            }
            else
                return;
        }
        public void EquipItem()
        {
            PlayerController player = GameManager.instance.player;
            
            if (player.inventory.artifactItems.Length < 4)
            {
                InventoryUI.Instance.ResetSlots();
                player.inventory.AddArtifactData(InventoryUI.Instance.choiceSlot.slot.item);
                player.inventory.Equip(InventoryUI.Instance.choiceSlot.slot.item);
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
            PlayerController player = GameManager.instance.player;
            var inventory = InventoryUI.Instance;
            const int mineralCount = 1;
            
            if (player.inventory.items[inventory.choiceSlot.id].item.itemType == Item.ItemType.Mineral)
            {
                if (player.inventory.items[inventory.choiceSlot.id].count > mineralCount)
                {
                    player.inventory.items[inventory.choiceSlot.id].count -= mineralCount;
                    EventManager.Notify(new ItemUsedEvent(player.inventory.items[inventory.choiceSlot.id].item, mineralCount));
                }
                else if(player.inventory.items[inventory.choiceSlot.id].count == mineralCount)
                    player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
                inventory.ResetSlots();
                inventory.UpdateSlots();
                _stoneCount.UpCount(mineralCount);
            }
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatTenMineral()
        {
            PlayerController player = GameManager.instance.player;
            var inventory = InventoryUI.Instance;
            const int mineralCount = 10;
            
            if (player.inventory.items[inventory.choiceSlot.id].item.itemType == Item.ItemType.Mineral)
            {
                if (player.inventory.items[inventory.choiceSlot.id].count > mineralCount)
                {
                    player.inventory.items[inventory.choiceSlot.id].count -= mineralCount;
                    EventManager.Notify(new ItemUsedEvent(player.inventory.items[inventory.choiceSlot.id].item, mineralCount));
                }
                else if (player.inventory.items[inventory.choiceSlot.id].count == mineralCount)
                    player.inventory.RemoveItemData(player.inventory.items[inventory.choiceSlot.id].item);
                
                inventory.ResetSlots();
                inventory.UpdateSlots();
                _stoneCount.UpCount(mineralCount);
            }
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void EatAllMineral()
        {
            PlayerController player = GameManager.instance.player;
            var inventory = InventoryUI.Instance;
            int count = player.inventory.items[inventory.choiceSlot.id].count;
            
            player.inventory.items[inventory.choiceSlot.id].count -= count;
            EventManager.Notify(new ItemUsedEvent(player.inventory.items[inventory.choiceSlot.id].item, count));
            _stoneCount.UpCount(count);
            player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
            inventory.ResetSlots();
            inventory.UpdateSlots();
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void UseItem()
        {
            PlayerController player = GameManager.instance.player;
            var chosenItem = player.inventory.items[InventoryUI.Instance.choiceSlot.id].item;
            
            PlayerStatus playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            player.inventory.items[InventoryUI.Instance.choiceSlot.id].item.Active(playerStatus);
            player.inventory.DecreaseItemCount(chosenItem);
            InventoryUI.Instance.ResetSlots();
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        public void AbandonItem()
        {
            GameManager.instance.player.inventory.RemoveItemData(InventoryUI.Instance.choiceSlot.slot.item);
            _rightClickMenu.SetUI();
            _rightClickMenu.DeleteMenu();
        }
        
    }
}
