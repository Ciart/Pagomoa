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

        private InventorySlot slot => GetComponent<InventorySlot>();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var gameManager = GameManager.instance;
                var uiManager = UIManager.instance; 
                
                uiManager.bookUI.inventoryUI.choiceSlot = slot;
                var choiceSlot = gameManager.player.inventory.items[uiManager.bookUI.inventoryUI.choiceSlot.id];
                var itemType = choiceSlot.GetSlotItem().type;
                Vector3 mousePosition = new Vector3(eventData.position.x + 5, eventData.position.y);
                rightClickMenu.SetUI();
                rightClickMenu.gameObject.transform.position = mousePosition;

                if (itemType == ItemType.Equipment)
                {
                    rightClickMenu.EquipmentMenu();
                }
                else if (itemType == ItemType.Mineral)
                {
                    rightClickMenu.MineralMenu(choiceSlot.GetSlotItemCount());
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

            if (player.inventory.items[inventory.choiceSlot.id].GetSlotItem() == null)
                return;

            if (player.inventory.items[inventory.choiceSlot.id].GetSlotItem().type == ItemType.Equipment)
                EquipItem();

            else if (player.inventory.items[inventory.choiceSlot.id].GetSlotItem().type == ItemType.Use)
            {
                var minusCount = player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount() - 1;
                
                player.inventory.items[inventory.choiceSlot.id].SetSlotItemCount(minusCount);;
                UseItem();
                if (player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount() == 0)
                {
                    player.inventory.SellItem(player.inventory.items[inventory.choiceSlot.id].GetSlotItem());
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
                player.inventory.AddArtifactData(inventory.choiceSlot.GetSlotItem());
                player.inventory.Equip(inventory.choiceSlot.GetSlotItem());
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
            
            if (player.inventory.items[inventory.choiceSlot.id].GetSlotItem().type == ItemType.Mineral)
            {
                if (player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount() > mineralCount)
                {
                    var minusCount = player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount() - mineralCount;
                    
                    player.inventory.items[inventory.choiceSlot.id].SetSlotItemCount(minusCount);
                    EventManager.Notify(new ItemUsedEvent(player.inventory.items[inventory.choiceSlot.id].GetSlotItem(), mineralCount));
                }
                else if(player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount() == mineralCount)
                    player.inventory.RemoveItemData(inventory.choiceSlot.GetSlotItem());
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
            
            if (player.inventory.items[inventory.choiceSlot.id].GetSlotItem().type == ItemType.Mineral)
            {
                if (player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount() > mineralCount)
                {
                    var minusCount = player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount() - mineralCount;
                    
                    player.inventory.items[inventory.choiceSlot.id].SetSlotItemCount(mineralCount);
                    EventManager.Notify(new ItemUsedEvent(player.inventory.items[inventory.choiceSlot.id].GetSlotItem(), mineralCount));
                }
                else if (player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount() == mineralCount)
                    player.inventory.RemoveItemData(player.inventory.items[inventory.choiceSlot.id].GetSlotItem());
                
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
            int count = player.inventory.items[inventory.choiceSlot.id].GetSlotItemCount();
            
            player.inventory.items[inventory.choiceSlot.id].SetSlotItemCount(count);
            EventManager.Notify(new ItemUsedEvent(player.inventory.items[inventory.choiceSlot.id].GetSlotItem(), count));
            stoneCount.UpCount(count);
            player.inventory.RemoveItemData(inventory.choiceSlot.GetSlotItem());
            inventory.ResetSlots();
            inventory.UpdateSlots();
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        public void UseItem()
        {
            var player = GameManager.instance.player;
            var inventory = UIManager.instance.bookUI.inventoryUI;
            var chosenItem = player.inventory.items[inventory.choiceSlot.id].GetSlotItem();
            
            player.inventory.items[inventory.choiceSlot.id].GetSlotItem().Use();
            player.inventory.DecreaseItemCount(chosenItem);
            inventory.ResetSlots();
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        public void AbandonItem()
        {
            var inventory = UIManager.instance.bookUI.inventoryUI;
            var player = GameManager.instance.player;
            
            player.inventory.RemoveItemData(inventory.choiceSlot.GetSlotItem());
            rightClickMenu.SetUI();
            rightClickMenu.DeleteMenu();
        }
        
    }
}
