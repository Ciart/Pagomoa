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
            var choiceSlot = inventory.inventorySlots[inventoryUI.chosenSlot.id];
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

            if (inventory.inventorySlots[targetID].GetSlotItem().id == "") return;

            if (inventory.inventorySlots[targetID].GetSlotItem().type == ItemType.Equipment)
                EquipItem();

            else if (inventory.inventorySlots[targetID].GetSlotItem().type == ItemType.Use)
            {
                var minusCount = inventory.inventorySlots[targetID].GetSlotItemCount() - 1;
                
                inventory.inventorySlots[targetID].SetSlotItemCount(minusCount);;
                UseItem();
                if (inventory.inventorySlots[targetID].GetSlotItemCount() == 0)
                {
                    inventory.SellItem(inventory.inventorySlots[targetID].GetSlotItem());
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
                inventory.AddArtifactData(inventoryUI.chosenSlot.GetSlotItem());
                inventory.Equip(inventoryUI.chosenSlot.GetSlotItem());
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
            
            if (inventory.inventorySlots[targetID].GetSlotItem().type == ItemType.Mineral)
            {
                if (inventory.inventorySlots[targetID].GetSlotItemCount() > mineralCount)
                {
                    var minusCount = inventory.inventorySlots[targetID].GetSlotItemCount() - mineralCount;
                    
                    inventory.inventorySlots[targetID].SetSlotItemCount(minusCount);
                    EventManager.Notify(new ItemUsedEvent(inventory.inventorySlots[targetID].GetSlotItem(), mineralCount));
                }
                else if(inventory.inventorySlots[targetID].GetSlotItemCount() == mineralCount)
                    inventory.RemoveItemData(UIManager.instance.bookUI.GetInventoryUI().chosenSlot.GetSlotItem());
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
            
            if (inventory.inventorySlots[targetID].GetSlotItem().type == ItemType.Mineral)
            {
                if (inventory.inventorySlots[targetID].GetSlotItemCount() > mineralCount)
                {
                    var minusCount = inventory.inventorySlots[targetID].GetSlotItemCount() - mineralCount;
                    
                    inventory.inventorySlots[targetID].SetSlotItemCount(mineralCount);
                    EventManager.Notify(new ItemUsedEvent(inventory.inventorySlots[targetID].GetSlotItem(), mineralCount));
                }
                else if (inventory.inventorySlots[targetID].GetSlotItemCount() == mineralCount)
                    inventory.RemoveItemData(inventory.inventorySlots[targetID].GetSlotItem());
                
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
            int count = inventory.inventorySlots[inventoryUI.chosenSlot.id].GetSlotItemCount();
            
            inventory.inventorySlots[inventoryUI.chosenSlot.id].SetSlotItemCount(count);
            EventManager.Notify(
                new ItemUsedEvent(inventory.inventorySlots[inventoryUI.chosenSlot.id].GetSlotItem(), count));
            stoneCount.UpCount(count);
            inventory.RemoveItemData(inventoryUI.chosenSlot.GetSlotItem());
            inventoryUI.UpdateSlots();
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        public void UseItem()
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var chosenItem = inventoryUI.chosenSlot.GetSlotItem();
            
            inventory.inventorySlots[inventoryUI.chosenSlot.id].GetSlotItem().DisplayUseEffect();
            inventory.DecreaseItemCount(chosenItem);
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
        public void AbandonItem()
        {
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            
            if (inventoryUI.chosenSlot.GetSlotItem().type != ItemType.Equipment ||
                inventoryUI.chosenSlot.GetSlotItem().type != ItemType.Inherent)
            {
                inventoryUI.chosenSlot.GetSlotItem().ClearItemProperty();
                inventoryUI.chosenSlot.ResetSlot();
            }
            
            UIManager.instance.bookUI.GetRightClickMenu().DeleteMenu();
        }
    }
}
