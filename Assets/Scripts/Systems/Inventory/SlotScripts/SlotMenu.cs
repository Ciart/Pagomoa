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
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        
        public void UnEquipArtifact()
        {
            var inventory = Game.Instance.player.inventory;
            inventory.UnEquipArtifact(inventorySlotUI);
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        
        public void EatMineral(int count)
        {
            var inventory = Game.Instance.player.inventory;
            var mineralCount = count;
            
            inventory.DecreaseItemBySlotID(inventorySlotUI.GetSlotID(), mineralCount);
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }

        public void UseItem()
        {
            var inventory = Game.Instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.GetSlotID());
            
            if (targetSlot == null) return;
            if (inventorySlotUI.GetSlotType() != SlotType.Inventory) return;
            
            targetSlot.GetSlotItem().DisplayUseEffect();
            inventory.DecreaseItemBySlotID(inventorySlotUI.GetSlotID());
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        
        public void AbandonItem()
        {
            var inventory = Game.Instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, inventorySlotUI.GetSlotID());
            
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
