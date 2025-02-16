using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SlotMenu : MonoBehaviour
    {
        private InventorySlotUI inventorySlotUI => GetComponent<InventorySlotUI>();
        
        // TODO : 장비 착용하는 법
        public void EquipArtifact()
        {
            var inventory = Game.Instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.GetSlotID());
            
            if (targetSlot.GetSlotItemID() == "") return;
            
            inventory.EquipArtifact(inventorySlotUI);
            
            Game.Instance.UI.bookUI.GetRightClickMenu().DeleteMenu();
        }
        
        public void UnEquipArtifact()
        {
            var inventory = Game.Instance.player.inventory;
            inventory.UnEquipArtifact(inventorySlotUI);
            
            Game.Instance.UI.bookUI.GetRightClickMenu().DeleteMenu();
        }
        
        public void EatMineral(int count)
        {
            var inventory = Game.Instance.player.inventory;
            var mineralCount = count;
            
            inventory.DecreaseItemBySlotID(inventorySlotUI.GetSlotID(), mineralCount);
            
            Game.Instance.UI.bookUI.GetRightClickMenu().DeleteMenu();
        }

        public void UseItem()
        {
            var inventory = Game.Instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.GetSlotID());
            
            if (inventorySlotUI.GetSlotType() != SlotType.Inventory) return;
            
            var slotItem = targetSlot.GetSlotItem(); 
            if (slotItem == null) return; 
            
            slotItem.DisplayUseEffect();
            if (slotItem.type == ItemType.Use)
                inventory.DecreaseItemBySlotID(inventorySlotUI.GetSlotID());
            
            Game.Instance.UI.bookUI.GetRightClickMenu().DeleteMenu();
        }
        
        public void AbandonItem()
        {
            var inventory = Game.Instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.GetSlotID());
            
            if (targetSlot.GetItemType() != ItemType.Equipment ||
                targetSlot.GetItemType() != ItemType.Inherent)
            {
                if (inventorySlotUI.GetSlotType() == SlotType.Inventory)
                {
                    inventory.RemoveItem(inventorySlotUI);    
                }
            }
            
            Game.Instance.UI.bookUI.GetRightClickMenu().DeleteMenu();
        }
    }
}
