using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using TMPro;
using Ciart.Pagomoa.Entities;

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

        [SerializeField] private TextMeshProUGUI hpValueText;
        [SerializeField] private TextMeshProUGUI damageValueText;
        [SerializeField] private TextMeshProUGUI oxygenValueText;
        [SerializeField] private TextMeshProUGUI deffenseValueText;
        [SerializeField] private TextMeshProUGUI hungerValueText;
        [SerializeField] private TextMeshProUGUI moveSpeedValueText;
        [SerializeField] private TextMeshProUGUI sightValueText;
        [SerializeField] private TextMeshProUGUI digValueText;

        private void InitInventorySlotUI(Inventory inventoryData)
        {
            for (int i = 0; i < Inventory.MaxInventorySlots; i++)
            {
                var spawnedSlot = Instantiate(instanceInventorySlotUI, inventorySlotParent.transform);
                spawnedSlot.SetSlotID(i);
                spawnedSlot.SetSlot(inventoryData.FindSlot(SlotType.Inventory, i));
                spawnedSlot.gameObject.SetActive(true);
                
                _inventoryUISlots.Add(spawnedSlot);
            }
        }

        private void InitArtifactSlotUI(Inventory inventoryData)
        {
            for (int i = 0; i < Inventory.MaxArtifactSlots; i++)
            {
                var spawnedSlot = Instantiate(instanceArtifactSlotUI, artifacSlotParent.transform);
                spawnedSlot.SetSlotID(i);
                spawnedSlot.SetSlot(inventoryData.FindSlot(SlotType.Artifact, i));
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
            if (Game.Instance.player == null) return;
            var inventory = Game.Instance.player.inventory;
            
            
            if (_inventoryUISlots.Count == 0) InitInventorySlotUI(inventory);
            
            var slotList = inventory.GetSlots(SlotType.Inventory);
            
            for (int i = 0; i < Inventory.MaxInventorySlots; i++)
            {
                _inventoryUISlots[i].SetSlot(slotList[i]);
                _inventoryUISlots[i].SetSlotID(i);
            }
        }

        private void UpdateArtifactSlot()
        {
            if (Game.Instance.player == null) return;
            var inventory = Game.Instance.player.inventory;
            
            if (_artifactSlots.Count == 0) InitArtifactSlotUI(inventory);
            
            var slotList = inventory.GetSlots(SlotType.Artifact);

            for (int i = 0; i < Inventory.MaxArtifactSlots; i++)
            {
                _artifactSlots[i].SetSlot(slotList[i]);
                _artifactSlots[i].SetSlotID(i);
            }
        }
        private void UpdateHPValueText(EntityDamagedEventArgs args)
        {
            hpValueText.text = $"{Game.Instance.player.entityController.health:F0}";
        }

        private void UpdateDamageValueText()
        {
            damageValueText.text = $"{Game.Instance.player.Attack:F0}";
        }

        private void UpdateOxygenValueText(float currentValue, float maxValue)
        {
            oxygenValueText.text = $"{currentValue:F1}";
        }

        private void UpdateDeffenseValueText()
        {
            deffenseValueText.text = $"{Game.Instance.player.Defense:F0}";
        }

        private void UpdateHungryValueText(float currentValue, float maxValue)
        {
            hungerValueText.text = $"{currentValue:F1}";
        }

        private void UpdateMoveSpeedValueText()
        {
            moveSpeedValueText.text = $"{Game.Instance.player.Speed:F1}";
        }

        private void UpdateSightValueText()
        {
            sightValueText.text = $"{Game.Instance.player.status.sight:F1}";
        }

        private void UpdateDigValueText()
        {
            digValueText.text = $"{Game.Instance.player.status.digSpeed:F1}";
        }

        private void UpdateAllStatusText()
        {
            var player = Game.Instance.player;
            if (player == null) return;
            if (player.status == null) return;
            UpdateOxygenValueText(player.status.Oxygen, 0);
            UpdateHungryValueText(player.status.Hungry, 0);
            UpdateSightValueText();
            UpdateDigValueText();

            UpdateHPValueText(null);
            UpdateDamageValueText();
            UpdateDeffenseValueText();
            UpdateMoveSpeedValueText();
        }

        private void OnEnable()
        {
            UpdateInventorySlot();
            UpdateArtifactSlot();
            EventManager.AddListener<UpdateInventory>(UpdateInventoryUI);
            EventManager.AddListener<PlayerSpawnedEvent>(UpdatePlayerStatValueUI);

            var player = Game.Instance.player;
            if (player != null)
                UpdatePlayerStatValueUI(new PlayerSpawnedEvent(player));
        }

        private void UpdatePlayerStatValueUI(PlayerSpawnedEvent @event)
        {
            var player = Game.Instance.player;

            UpdateAllStatusText();

            player.status.oxygenAlter.AddListener(UpdateOxygenValueText);
            player.status.hungryAlter.AddListener(UpdateHungryValueText);
            player.status.sightChange += UpdateSightValueText;
            player.status.digSpeedChange += UpdateDigValueText;

            player.entityController.damaged += UpdateHPValueText;
            player.entityController.attackChanged += UpdateDamageValueText;
            player.entityController.deffenseChanged += UpdateDeffenseValueText;
            player.entityController.speedChanged += UpdateMoveSpeedValueText;
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<UpdateInventory>(UpdateInventoryUI);

            //var player = Game.Instance.player;

            //player.status.oxygenAlter.RemoveListener(UpdateOxygenValueText);
            //player.status.hungryAlter.RemoveListener(UpdateHungryValueText);
            //player.status.sightChange -= UpdateSightValueText;
            //player.status.digSpeedChange -= UpdateDigValueText;

            //player.entityController.damaged -= UpdateHPValueText;
            //player.entityController.attackChanged -= UpdateDamageValueText;
            //player.entityController.deffenseChanged -= UpdateDeffenseValueText;
            //player.entityController.speedChanged -= UpdateMoveSpeedValueText;
        }
    }
}
