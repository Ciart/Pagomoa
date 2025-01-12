using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ClickToSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private StoneCount stoneCount;

        private InventorySlot inventorySlot => GetComponent<InventorySlot>();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            if (inventorySlot.slot.GetSlotItemID() == "") return;
            
            var hover = UIManager.instance.bookUI.GetHoverItemInfo();
            
            if (hover.isActiveAndEnabled) hover.gameObject.SetActive(false);
            
            var rightClickMenu = UIManager.instance.bookUI.GetRightClickMenu();
            rightClickMenu.DeleteMenu();
            rightClickMenu.transform.position = inventorySlot.transform.position;
            
            UIManager.instance.GetUIContainer().SetChosenSlot(inventorySlot);
            var itemType = inventorySlot.slot.GetSlotItem().type;
            
            switch (itemType)
            {
                case ItemType.Equipment:
                    rightClickMenu.EquipmentMenu();
                    break;    
                case ItemType.Mineral:
                    rightClickMenu.MineralMenu(inventorySlot.slot.GetSlotItemCount());
                    break;
                case ItemType.Use:
                    rightClickMenu.UseMenu();
                    break;
                case ItemType.Inherent:
                    rightClickMenu.InherentMenu();
                    break;
            }
        }
        public void EquipCheck()
        {
            var inventory = GameManager.instance.player.inventory;
            var targetID = inventorySlot.GetSlotID();
            
            if (inventory.inventoryItems[targetID].GetSlotItem().id == "") return;

            if (ItemType.Equipment 
                == inventory.inventoryItems[targetID].GetSlotItem().type)
            {
                EquipItem();
            }
            else if (ItemType.Use 
                     == inventory.inventoryItems[targetID].GetSlotItem().type)
            {
                UseItem();
            }
            
            UIManager.instance.GetUIContainer().SetChosenSlot(null);
        }
        // TODO : 장비 착용하는 법
        public void EquipItem() { UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu(); }
        
        public void EatMineral(int count)
        {
            var inventory = GameManager.instance.player.inventory;
            var mineralCount = count;
            
            inventory.DecreaseItemBySlotID(inventorySlot, mineralCount);
            
            //stoneCount.UpCount(mineralCount);
            UIManager.instance.bookUI.GetInventoryUI().UpdateInventorySlot();
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }

        public void UseItem()
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            
            if (inventorySlot.GetSlotType() != SlotType.Inventory) return;
            
            inventorySlot.slot.GetSlotItem().DisplayUseEffect();
            inventory.DecreaseItemBySlotID(inventorySlot);
            inventoryUI.UpdateInventorySlot();
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        
        public void AbandonItem()
        {
            var inventory = GameManager.instance.player.inventory;
            
            if (inventory.FindInventorySlotByID(inventorySlot).GetSlotItem().type != ItemType.Equipment ||
                inventory.FindInventorySlotByID(inventorySlot).GetSlotItem().type != ItemType.Inherent)
            {
                if (inventorySlot.GetSlotType() == SlotType.Inventory)
                {
                    inventory.RemoveItemBySlotID(inventorySlot);    
                }
            }
            
            UIManager.instance.bookUI.GetInventoryUI().UpdateInventorySlot();
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
            UIManager.instance.GetUIContainer().SetChosenSlot(null);
        }
    }
}
