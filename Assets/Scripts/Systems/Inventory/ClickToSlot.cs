using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ClickToSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RightClickMenu rightClickMenu;
        [SerializeField] private StoneCount stoneCount;

        private InventorySlotUI slot => GetComponent<InventorySlotUI>();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var gameManager = GameManager.instance;
                var uiManager = UIManager.instance; 
                
                uiManager.bookUI.inventoryUI.choiceSlot = slot;
                var choiceSlot = gameManager.player.inventory.items[uiManager.bookUI.inventoryUI.choiceSlot.id];
                var itemType = choiceSlot.item.type;
                Vector3 mousePosition = new Vector3(eventData.position.x + 5, eventData.position.y);
                rightClickMenu.SetUI();
                rightClickMenu.gameObject.transform.position = mousePosition;

                if (itemType == ItemType.Equipment)
                {
                    rightClickMenu.EquipmentMenu();
                }
                else if (itemType == ItemType.Mineral)
                {
                    rightClickMenu.MineralMenu(choiceSlot.count);
                }
                else if (itemType == ItemType.Use)
                {
                    rightClickMenu.UseMenu();
                }
                else if (itemType == ItemType.Inherent)
                {
                    rightClickMenu.InherentMenu();
                }
            }
            else
                return;
        }
        public void EquipCheck()
        {
            var inventory = UIManager.instance.bookUI.inventoryUI;
            PlayerController player = GameManager.instance.player;

            if (player.inventory.items[inventory.choiceSlot.id].item == null)
                return;

            if (player.inventory.items[inventory.choiceSlot.id].item.type == ItemType.Equipment)
                EquipItem();

            else if (player.inventory.items[inventory.choiceSlot.id].item.type == ItemType.Use)
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
            var player = GameManager.instance.player;
            var inventory = UIManager.instance.bookUI.inventoryUI;
            
            if (player.inventory.artifactItems.Length < 4)
            {
                inventory.ResetSlots();
                player.inventory.AddArtifactData(inventory.choiceSlot.slot.item);
                player.inventory.Equip(inventory.choiceSlot.slot.item);
                inventory.UpdateSlots();
                inventory.SetArtifactSlots();
            }
            else return;
            
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        public void EatMineral()
        {
            PlayerController player = GameManager.instance.player;
            var inventory = UIManager.instance.bookUI.inventoryUI;
            const int mineralCount = 1;
            
            if (player.inventory.items[inventory.choiceSlot.id].item.type == ItemType.Mineral)
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
                stoneCount.UpCount(mineralCount);
            }
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        public void EatTenMineral()
        {
            var player = GameManager.instance.player;
            var inventory = UIManager.instance.bookUI.inventoryUI;
            const int mineralCount = 10;
            
            if (player.inventory.items[inventory.choiceSlot.id].item.type == ItemType.Mineral)
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
                stoneCount.UpCount(mineralCount);
            }
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        public void EatAllMineral()
        {
            var player = GameManager.instance.player;
            var inventory = UIManager.instance.bookUI.inventoryUI;
            int count = player.inventory.items[inventory.choiceSlot.id].count;
            
            player.inventory.items[inventory.choiceSlot.id].count -= count;
            EventManager.Notify(new ItemUsedEvent(player.inventory.items[inventory.choiceSlot.id].item, count));
            stoneCount.UpCount(count);
            player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
            inventory.ResetSlots();
            inventory.UpdateSlots();
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        public void UseItem()
        {
            var player = GameManager.instance.player;
            var inventory = UIManager.instance.bookUI.inventoryUI;
            var chosenItem = player.inventory.items[inventory.choiceSlot.id].item;
            
            player.inventory.items[inventory.choiceSlot.id].item.Use();
            player.inventory.DecreaseItemCount(chosenItem);
            inventory.ResetSlots();
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        public void AbandonItem()
        {
            var inventory = UIManager.instance.bookUI.inventoryUI;
            var player = GameManager.instance.player;
            
            player.inventory.RemoveItemData(inventory.choiceSlot.slot.item);
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        
    }
}
