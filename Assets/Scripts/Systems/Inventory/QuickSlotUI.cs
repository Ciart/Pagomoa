using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlotUI : MonoBehaviour
    {
        private int _selectedSlotIndex = 0;

        public QuickSlot chosenSlot;
        private List<QuickSlot> _quickSlots = new List<QuickSlot>();
        
        [Header("퀵 슬롯 스프라이트")]
        [SerializeField] private Sprite _selectedQuickSlotSprite;
        [SerializeField] private Sprite _normalQuickSlotSprite;
        
        [SerializeField] private QuickSlot sellingQuickSlot;

        private PlayerInput _playerInput;
        private QuickSlot _findSlot;

        public void InitQuickSlots()
        {
            var index = 0;
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).TryGetComponent<QuickSlot>(out var quickSlot);
                if (!quickSlot) continue;
                
                inventory.quickItems[index] = quickSlot;
                inventory.quickItems[index].id = index;
                _quickSlots.Add(inventory.quickItems[index]);
                index++;
                
                if (index == Inventory.MaxQuickSlots) break;
            }
        }

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
        
        
        private void OnQuickSlotChanged(QuickSlotChangedEvent e)
        {
            var inventory = GameManager.instance.player.inventory;

            for (int i = 0; i < Inventory.MaxQuickSlots; i++)
            {
                _quickSlots[i].SetSlot(inventory.quickItems[i]);
            }

            if (e.dependentID is -1 || e.quickSlotID is -1) return;
            
            foreach (var slot in _quickSlots)
            {
                if (slot.dependentID == e.dependentID)
                {
                    if (slot.id == e.quickSlotID) continue;
                    
                    slot.ResetSlot();
                    slot.dependentID = -1;
                }
            }
        }
        
        private void OnItemCountChanged(ItemCountChangedEvent e)
        {
            var inventoryItem = e;

            for (int i = 0; i < Inventory.MaxQuickSlots; i++)
            {
                if (inventoryItem.item.id == _quickSlots[i].GetSlotItem().id)
                    _quickSlots[i].SetSlotItemCount(e.count);
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
            EventManager.AddListener<QuickSlotChangedEvent>(OnQuickSlotChanged);
            EventManager.AddListener<ItemCountChangedEvent>(OnItemCountChanged);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
            EventManager.RemoveListener<QuickSlotChangedEvent>(OnQuickSlotChanged);
            EventManager.RemoveListener<ItemCountChangedEvent>(OnItemCountChanged);
        }
        
        // public void SetQuickSlotItemCount(Item data)
        // {
        //     int itemsIdx = Array.FindIndex(GameManager.player.inventoryDB.items, element => element.item == data);
        //     int quickSlotsIndex = Array.FindIndex(GameManager.player.inventoryDB.quickSlots, element => element.item == data);
        //     
        //     if (itemsIdx != -1  && quickSlotsIndex != -1)
        //     {
        //         InventoryItem item = GameManager.player.inventoryDB.items[itemsIdx];
        //         InventoryItem quickSlotItem = GameManager.player.inventoryDB.quickSlots[quickSlotsIndex];
        //         
        //         for (int i = 0; i < GameManager.player.inventoryDB.quickSlots.Length; i++)
        //         {
        //             if (quickSlotsIndex == _quickSlotUIs[i].id)
        //             {
        //                 _findSlotUI = _quickSlotUIs[i];
        //             }
        //         }
        //         
        //         if (item.count > 1)
        //         {
        //             _findSlotUI.itemCount.text = (item.count - 1).ToString();
        //         }
        //         else if (item.count == 1)
        //         {
        //             quickSlotItem.item = null;
        //             quickSlotItem.count = 0;
        //             _findSlotUI.itemImage.sprite = _findSlotUI.transparentImage;
        //             _findSlotUI.itemCount.text = null;
        //         }
        //     }
        // }
        
        private void UseQuickSlot()
        {
            GameManager.instance.player.inventory.UseQuickSlotItem(_selectedSlotIndex);
        }
        
        private void UpdateQuickSlot()
        {
            for (var i = 0; i < _quickSlots.Count; i++)
            {
                var slot = _quickSlots[i];
                var index = i;
                
                if (index + 1 == _selectedSlotIndex)
                {
                    slot.slotImage.sprite = _selectedQuickSlotSprite;
                }
                else
                {
                    slot.slotImage.sprite = _normalQuickSlotSprite;
                }
            }
        }
        
        public void SelectQuickSlot(int index)
        {
            _selectedSlotIndex = index;
            UpdateQuickSlot();
        }
    }
}
