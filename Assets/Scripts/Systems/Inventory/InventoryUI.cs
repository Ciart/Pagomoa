using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private RectTransform inventorySlotParent;
        [SerializeField] private InventorySlotUI instanceInventorySlotUI;
        [SerializeField] private RectTransform artifacSlotParent;
        [SerializeField] private ArtifactSlotUI instanceArtifactSlotUI;
        private List<InventorySlotUI> _inventoryUISlots = new List<InventorySlotUI>(Inventory.MaxInventorySlots);
        private List<ArtifactSlotUI> _artifactSlots = new List<ArtifactSlotUI>(Inventory.MaxArtifactSlots);

        private void InitInventorySlotUI()
        {
            var inventory = GameManager.instance.player.inventory;

            for (int i = 0; i < Inventory.MaxInventorySlots; i++)
            {
                var spawnedSlot = Instantiate(instanceInventorySlotUI, inventorySlotParent.transform);
                spawnedSlot.SetSlotID(i);
                spawnedSlot.SetSlot(inventory.FindSlot(SlotType.Inventory, i));
                spawnedSlot.gameObject.SetActive(true);
                
                _inventoryUISlots.Add(spawnedSlot);
            }
            UIManager.instance.bookUI.gameObject.SetActive(false);
        } 
        private void UpdateInventoryUI(UpdateInventory e) { UpdateInventorySlot(); }  
        
        public void UpdateInventorySlot() // List안의 Item 전체 인벤토리에 출력
        {
            if (_inventoryUISlots.Count == 0) InitInventorySlotUI();
            
            var slotList = GameManager.instance.player.inventory.GetSlots(SlotType.Inventory);
            if (slotList == null) return;
            
            for (int i = 0; i < Inventory.MaxInventorySlots; i++)
            {
                _inventoryUISlots[i].SetSlot(slotList[i]);
            }
        }
        
        private void OnEnable()
        {
            UpdateInventorySlot();
            EventManager.AddListener<UpdateInventory>(UpdateInventoryUI);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<UpdateInventory>(UpdateInventoryUI);
        }
    }
}
