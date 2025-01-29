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
        private readonly List<InventorySlotUI> _inventoryUISlots 
            = new List<InventorySlotUI>(Inventory.MaxInventorySlots);
        private readonly List<ArtifactSlotUI> _artifactSlots 
            = new List<ArtifactSlotUI>(Inventory.MaxArtifactSlots);

        private void InitInventorySlotUI()
        {
            var inventory = Game.Instance.player.inventory;

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

        private void InitArtifactSlotUI()
        {
            var inventory = Game.Instance.player.inventory;

            for (int i = 0; i < Inventory.MaxArtifactSlots; i++)
            {
                var spawnedSlot = Instantiate(instanceArtifactSlotUI, artifacSlotParent.transform);
                spawnedSlot.SetSlotID(i);
                spawnedSlot.SetSlot(inventory.FindSlot(SlotType.Artifact, i));
                spawnedSlot.gameObject.SetActive(true);
                
                _artifactSlots .Add(spawnedSlot);
            }
        }

        private void UpdateInventoryUI(UpdateInventory e)
        {
            UpdateInventorySlot();
            UpdateArtifactSlot();
        }  
        
        private void UpdateInventorySlot() // List안의 Item 전체 인벤토리에 출력
        {
            if (_inventoryUISlots.Count == 0) InitInventorySlotUI();
            
            var slotList = Game.Instance.player.inventory.GetSlots(SlotType.Inventory);
            if (slotList == null) return;
            
            for (int i = 0; i < Inventory.MaxInventorySlots; i++)
            {
                _inventoryUISlots[i].SetSlot(slotList[i]);
                _inventoryUISlots[i].SetSlotID(i);
            }
        }

        private void UpdateArtifactSlot()
        {
            if (_artifactSlots.Count == 0) InitArtifactSlotUI();
            
            var slotList = Game.Instance.player.inventory.GetSlots(SlotType.Artifact);
            if (slotList == null) return;

            for (int i = 0; i < Inventory.MaxArtifactSlots; i++)
            {
                _artifactSlots[i].SetSlot(slotList[i]);
                _artifactSlots[i].SetSlotID(i);
            }
        }
        
        private void OnEnable()
        {
            UpdateInventorySlot();
            UpdateArtifactSlot();
            EventManager.AddListener<UpdateInventory>(UpdateInventoryUI);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<UpdateInventory>(UpdateInventoryUI);
        }
    }
}
