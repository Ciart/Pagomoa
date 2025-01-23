using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SlotMenu : MonoBehaviour
    {
        private InventorySlotUI inventorySlotUI => GetComponent<InventorySlotUI>();
        
        public void EquipCheck()
        {
            var inventory = GameManager.instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.slotID);
            
            if (targetSlot == null) return;
            if (targetSlot.GetSlotItemID() == "") return;

            if (ItemType.Equipment == targetSlot.GetItemType())
            {
                EquipItem();
            }
            else if (ItemType.Use == targetSlot.GetItemType())
            {
                UseItem();
            }
        }
        // TODO : 장비 착용하는 법
        public void EquipItem() { UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu(); }
        
        public void EatMineral(int count)
        {
            var inventory = GameManager.instance.player.inventory;
            var mineralCount = count;
            
            inventory.DecreaseItemBySlotID(inventorySlotUI, mineralCount);
            
            UIManager.instance.bookUI.GetInventoryUI().UpdateInventorySlot();
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }

        public void UseItem()
        {
            var inventory = GameManager.instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.slotID);
            
            if (targetSlot == null) return;
            if (inventorySlotUI.GetSlotType() != SlotType.Inventory) return;
            
            targetSlot.GetSlotItem().DisplayUseEffect();
            inventory.DecreaseItemBySlotID(inventorySlotUI);
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        
        public void AbandonItem()
        {
            var inventory = GameManager.instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.slotID);
            
            if (targetSlot == null) return;
            
            if (targetSlot.GetItemType() != ItemType.Equipment ||
                targetSlot.GetItemType() != ItemType.Inherent)
            {
                if (inventorySlotUI.GetSlotType() == SlotType.Inventory)
                {
                    inventory.RemoveItem(inventorySlotUI);    
                }
            }
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
    }
}
