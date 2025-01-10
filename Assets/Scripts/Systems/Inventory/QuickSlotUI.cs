using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using PlayerInput = Ciart.Pagomoa.Entities.Players.PlayerInput;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlotUI : MonoBehaviour
    {
        public QuickSlot chosenSlot;
        [SerializeField] private List<QuickSlot> _quickSlots;
        
        [Header("퀵 슬롯 스프라이트")]
        [SerializeField] private Sprite _selectedQuickSlotSprite;
        [SerializeField] private Sprite _normalQuickSlotSprite;

        private PlayerInput _playerInput;

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            _playerInput = e.player.GetComponent<PlayerInput>();
            
            _playerInput.Actions.Slot1.started += context => { SelectQuickSlot(1); };
            _playerInput.Actions.Slot2.started += context => { SelectQuickSlot(2); };
            _playerInput.Actions.Slot3.started += context => { SelectQuickSlot(3); };
            _playerInput.Actions.Slot4.started += context => { SelectQuickSlot(4); };
            _playerInput.Actions.Slot5.started += context => { SelectQuickSlot(5); };
            _playerInput.Actions.Slot6.started += context => { SelectQuickSlot(6); };
                
            _playerInput.Actions.UseQuickSlot.started += context => { UseQuickSlot(); };
        }
        
        // <Summary> 인벤토리 아이템 퀵슬롯 등록 시 중복 아이템 체크 <Summary>
        private void OnUpdateQuickSlot(ItemCountChangedEvent e)
        {
            UpdateQuickSlot();
        }

        public void UpdateQuickSlot()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < Inventory.MaxQuickSlots; i++)
            {
                _quickSlots[i].SetSlot(inventory.quickItems[i]);
            }
        }
        
        private void UseQuickSlot()
        {
            const string displayInput = "E";
            const string nameInput = "e";
            
            if (!chosenSlot) return;
            
            // key 패드 입력 'E', '1' ~ '6'
            var index = chosenSlot.id + 1;
            var playerInput = _playerInput.Actions.UseQuickSlot;
            
            var inventory = GameManager.instance.player.inventory;
            
            foreach (var input in playerInput.controls)
            {
                if (!input.IsPressed()) continue;

                if (input.displayName == displayInput 
                    || input.name == nameInput 
                    || input.displayName == index.ToString())
                {
                    
                    inventory.UseQuickSlotItem(chosenSlot.id);
                    break;
                }
            }
        }
        
        public void SelectQuickSlot(int index)
        {
            chosenSlot = _quickSlots[index - 1];
            UpdateSlotImage();
        }
        
        private void UpdateSlotImage()
        {
            for (var i = 0; i < _quickSlots.Count; i++)
            {
                var slot = _quickSlots[i];
                var index = i;
                
                if (index == chosenSlot.id)
                    slot.slotImage.sprite = _selectedQuickSlotSprite;
                else
                    slot.slotImage.sprite = _normalQuickSlotSprite;
            }
        }
        
        public void InitQuickSlots()
        {
            var index = 0;
            var inventory = GameManager.instance.player.inventory;

            _quickSlots = new List<QuickSlot>(Inventory.MaxQuickSlots);
            
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).TryGetComponent<QuickSlot>(out var quickSlot);
                if (!quickSlot) continue;
                
                quickSlot.id = index; 
                quickSlot.SetSlot(inventory.quickItems[index]);
                _quickSlots.Add(quickSlot);
                index++;

                if (index == Inventory.MaxQuickSlots) break;
            }
        }
        
        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
            EventManager.AddListener<ItemCountChangedEvent>(OnUpdateQuickSlot);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
            EventManager.RemoveListener<ItemCountChangedEvent>(OnUpdateQuickSlot);
        }
    }
}
