using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ClickToSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private StoneCount stoneCount;

        private InventorySlot slot => GetComponent<InventorySlot>();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            
            var hover = UIManager.instance.bookUI.GetHoverItemInfo();
            
            if (hover.isActiveAndEnabled)
            {
                hover.gameObject.SetActive(false);
            }
            
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var rightClickMenu = UIManager.instance.bookUI.GetRightClickMenu();
            rightClickMenu.DeleteMenu();
            rightClickMenu.transform.position = slot.transform.position;
            
            inventoryUI.chosenSlot = slot;
            var choiceSlot = inventory.inventoryItems[inventoryUI.chosenSlot.id];
            var itemType = choiceSlot.GetSlotItem().type;
            
            switch (itemType)
            {
                case ItemType.Equipment:
                    rightClickMenu.EquipmentMenu();
                    break;    
                case ItemType.Mineral:
                    rightClickMenu.MineralMenu(choiceSlot.GetSlotItemCount());
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
            var targetID = UIManager.instance.bookUI.GetInventoryUI().chosenSlot.id;
            var inventory = GameManager.instance.player.inventory;

            if (inventory.inventoryItems[targetID].GetSlotItem().id == "") return;

            if (inventory.inventoryItems[targetID].GetSlotItem().type == ItemType.Equipment)
                EquipItem();

            else if (inventory.inventoryItems[targetID].GetSlotItem().type == ItemType.Use)
            {
                var minusCount = inventory.inventoryItems[targetID].GetSlotItemCount() - 1;
                
                inventory.inventoryItems[targetID].SetSlotItemCount(minusCount);;
                UseItem();
                if (inventory.inventoryItems[targetID].GetSlotItemCount() == 0)
                {
                    inventory.SellItem(inventory.inventoryItems[targetID].GetSlotItem());
                }
                UIManager.instance.bookUI.GetInventoryUI().UpdateSlots();
            }
        }
        public void EquipItem()
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            
            if (inventory.artifactItems.Length < 4)
            {
                /*inventoryUI.ResetInventoryUI();*/
                inventory.AddArtifactData(inventoryUI.chosenSlot.slot.GetSlotItem());
                inventory.Equip(inventoryUI.chosenSlot.slot.GetSlotItem());
                inventoryUI.UpdateSlots();
                inventoryUI.SetArtifactSlots();
            }
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        public void EatMineral()
        {
            var inventory = GameManager.instance.player.inventory;
            var targetID = UIManager.instance.bookUI.GetInventoryUI().chosenSlot.id;
            const int mineralCount = 1;
            
            if (inventory.inventoryItems[targetID].GetSlotItem().type == ItemType.Mineral)
            {
                if (inventory.inventoryItems[targetID].GetSlotItemCount() > mineralCount)
                {
                    var minusCount = inventory.inventoryItems[targetID].GetSlotItemCount() - mineralCount;
                    
                    inventory.inventoryItems[targetID].SetSlotItemCount(minusCount);
                    EventManager.Notify(new ItemUsedEvent(inventory.inventoryItems[targetID].GetSlotItem(), mineralCount));
                }
                else if(inventory.inventoryItems[targetID].GetSlotItemCount() == mineralCount)
                    inventory.RemoveItemData(UIManager.instance.bookUI.GetInventoryUI().chosenSlot.slot.GetSlotItem());
                /*UIManager.instance.bookUI.GetInventoryUI().ResetInventoryUI();*/
                UIManager.instance.bookUI.GetInventoryUI().UpdateSlots();
                stoneCount.UpCount(mineralCount);
            }
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        public void EatTenMineral()
        {
            var inventory = GameManager.instance.player.inventory;
            var targetID = UIManager.instance.bookUI.GetInventoryUI().chosenSlot.id;
            const int mineralCount = 10;
            
            if (inventory.inventoryItems[targetID].GetSlotItem().type == ItemType.Mineral)
            {
                if (inventory.inventoryItems[targetID].GetSlotItemCount() > mineralCount)
                {
                    var minusCount = inventory.inventoryItems[targetID].GetSlotItemCount() - mineralCount;
                    
                    inventory.inventoryItems[targetID].SetSlotItemCount(mineralCount);
                    EventManager.Notify(new ItemUsedEvent(inventory.inventoryItems[targetID].GetSlotItem(), mineralCount));
                }
                else if (inventory.inventoryItems[targetID].GetSlotItemCount() == mineralCount)
                    inventory.RemoveItemData(inventory.inventoryItems[targetID].GetSlotItem());
                
                /*UIManager.instance.bookUI.GetInventoryUI().ResetInventoryUI();*/
                UIManager.instance.bookUI.GetInventoryUI().UpdateSlots();
                stoneCount.UpCount(mineralCount);
            }
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        public void EatAllMineral()
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            int count = inventory.inventoryItems[inventoryUI.chosenSlot.id].GetSlotItemCount();
            
            inventory.inventoryItems[inventoryUI.chosenSlot.id].SetSlotItemCount(count);
            EventManager.Notify(
                new ItemUsedEvent(inventory.inventoryItems[inventoryUI.chosenSlot.id].GetSlotItem(), count));
            stoneCount.UpCount(count);
            inventory.RemoveItemData(inventoryUI.chosenSlot.slot.GetSlotItem());
            inventoryUI.UpdateSlots();
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        public void UseItem()
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var chosenItem = inventoryUI.chosenSlot.slot.GetSlotItem();
            
            inventory.inventoryItems[inventoryUI.chosenSlot.id].GetSlotItem().DisplayUseEffect();
            inventory.DecreaseItemCount(chosenItem);
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        public void AbandonItem()
        {
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var playerSlot = GameManager.instance.player.inventory.inventoryItems[inventoryUI.chosenSlot.id];
            
            if (inventoryUI.chosenSlot.slot.GetSlotItem().type != ItemType.Equipment ||
                inventoryUI.chosenSlot.slot.GetSlotItem().type != ItemType.Inherent)
            {
                playerSlot.SetSlotItemID("");
                playerSlot.SetSlotItemCount(0);
            }
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
            EventManager.Notify(new ItemCountChangedEvent(playerSlot.GetSlotItemID(), playerSlot.GetSlotItemCount()));
        }
    }
}
