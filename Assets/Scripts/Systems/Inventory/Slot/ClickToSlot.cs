using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.EventSystems;

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
                var inventoryUI = UIManager.instance.bookUI.GetInventoryUI(); 
                
                inventoryUI.choiceSlot = slot;
                var choiceSlot = gameManager.player.inventory.inventorySlots[inventoryUI.choiceSlot.id];
                var itemType = choiceSlot.GetSlotItem().type;
                Vector3 mousePosition = new Vector3(eventData.position.x + 5, eventData.position.y);
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
        }
        public void EquipCheck()
        {
            var targetID = UIManager.instance.bookUI.GetInventoryUI().choiceSlot.id;
            var inventory = GameManager.instance.player.inventory;

            if (inventory.inventorySlots[targetID].GetSlotItem().id == "")
                return;

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
                inventoryUI.ResetSlots();
                inventory.AddArtifactData(inventoryUI.choiceSlot.GetSlotItem());
                inventory.Equip(inventoryUI.choiceSlot.GetSlotItem());
                inventoryUI.UpdateSlots();
                inventoryUI.SetArtifactSlots();
            }
            
            rightClickMenu.DeleteMenu();
        }
        public void EatMineral()
        {
            var inventory = GameManager.instance.player.inventory;
            var targetID = UIManager.instance.bookUI.GetInventoryUI().choiceSlot.id;
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
                    inventory.RemoveItemData(UIManager.instance.bookUI.GetInventoryUI().choiceSlot.GetSlotItem());
                UIManager.instance.bookUI.GetInventoryUI().ResetSlots();
                UIManager.instance.bookUI.GetInventoryUI().UpdateSlots();
                stoneCount.UpCount(mineralCount);
            }
            rightClickMenu.DeleteMenu();
        }
        public void EatTenMineral()
        {
            var inventory = GameManager.instance.player.inventory;
            var targetID = UIManager.instance.bookUI.GetInventoryUI().choiceSlot.id;
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
                
                UIManager.instance.bookUI.GetInventoryUI().ResetSlots();
                UIManager.instance.bookUI.GetInventoryUI().UpdateSlots();
                stoneCount.UpCount(mineralCount);
            }
            rightClickMenu.DeleteMenu();
        }
        public void EatAllMineral()
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            int count = inventory.inventorySlots[inventoryUI.choiceSlot.id].GetSlotItemCount();
            
            inventory.inventorySlots[inventoryUI.choiceSlot.id].SetSlotItemCount(count);
            EventManager.Notify(
                new ItemUsedEvent(inventory.inventorySlots[inventoryUI.choiceSlot.id].GetSlotItem(), count));
            stoneCount.UpCount(count);
            inventory.RemoveItemData(inventoryUI.choiceSlot.GetSlotItem());
            inventoryUI.UpdateSlots();
            
            rightClickMenu.DeleteMenu();
        }
        public void UseItem()
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var chosenItem = inventory.inventorySlots[inventoryUI.choiceSlot.id].GetSlotItem();
            
            inventory.inventorySlots[inventoryUI.choiceSlot.id].GetSlotItem().Use();
            inventory.DecreaseItemCount(chosenItem);
            
            rightClickMenu.DeleteMenu();
        }
        public void AbandonItem()
        {
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var inventory = GameManager.instance.player.inventory;
            
            inventory.RemoveItemData(inventoryUI.choiceSlot.GetSlotItem());
            
            rightClickMenu.DeleteMenu();
        }
        
    }
}
