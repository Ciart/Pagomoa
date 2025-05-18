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
    public class QuickUI : MonoBehaviour
    {
        [SerializeField] private List<QuickSlotUI> _quickSlots;
        private QuickSlotUI _chosenSlotUI;
        
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

            InitQuickSlots();
            UpdateQuickSlot();
        }
        
        // <Summary> 인벤토리 아이템 퀵슬롯 등록 시 중복 아이템 체크 <Summary>
        private void OnUpdateQuickSlot(UpdateInventoryEvent e) { UpdateQuickSlot(); }

        public void UpdateQuickSlot()
        {
            var slotList = Game.Instance.player.inventory.GetSlots(SlotType.Quick);
            
            if (slotList == null) return;
            for (int i = 0; i < Inventory.MaxQuickSlots; i++)
            {
                _quickSlots[i].SetSlot(slotList[i]);
            }
        }
        
        private void UseQuickSlot()
        {
            const string displayInput = "E";
            const string nameInput = "e";
            
            if (!_chosenSlotUI) return;
            
            // key 패드 입력 'E', '1' ~ '6'
            var index = _chosenSlotUI.GetSlotID() + 1;
            var playerInput = _playerInput.Actions.UseQuickSlot;
            
            var inventory = Game.instance.player.inventory;
            
            foreach (var input in playerInput.controls)
            {
                if (!input.IsPressed()) continue;

                if (input.displayName == displayInput 
                    || input.name == nameInput 
                    || input.displayName == index.ToString())
                {
                    
                    inventory.UseQuickSlotItem(_chosenSlotUI.GetSlotID());
                    break;
                }
            }
        }
        
        public void SelectQuickSlot(int index)
        {
            var slotID = index - 1;
            
            for (var i = 0; i < Inventory.MaxQuickSlots; i++)
            {
                if (_quickSlots[i].GetSlotID() == slotID)
                {
                    _quickSlots[i].SetSlotImage(_selectedQuickSlotSprite);
                    _chosenSlotUI = _quickSlots[i];
                }
                else
                    _quickSlots[i].SetSlotImage(_normalQuickSlotSprite);
            }
        }
        
        public void InitQuickSlots()
        {
            _quickSlots = new List<QuickSlotUI>(Inventory.MaxQuickSlots);
            var index = 0;
            var inventory = Game.Instance.player.inventory;
            var quickSlots = inventory.GetSlots(SlotType.Quick);
            
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).TryGetComponent<QuickSlotUI>(out var quickSlot);
                if (!quickSlot) continue;
                quickSlot.SetSlotID(index); 
                quickSlot.SetSlot(quickSlots[index]);
                _quickSlots.Add(quickSlot);
                index++;

                if (index == Inventory.MaxQuickSlots) break;
            }
        }
        
        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
            EventManager.AddListener<UpdateInventoryEvent>(OnUpdateQuickSlot);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
            EventManager.RemoveListener<UpdateInventoryEvent>(OnUpdateQuickSlot);
        }
    }
}
