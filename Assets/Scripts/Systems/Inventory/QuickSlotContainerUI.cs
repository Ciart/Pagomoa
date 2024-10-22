using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlotContainerUI : MonoBehaviour
    {
        private int _selectedSlotIndex = 0;
        
        public QuickSlotUI[] quickSlotUIs = new QuickSlotUI[Inventory.MaxQuickItems];
        
        [FormerlySerializedAs("_sellingQuickSlot")] [SerializeField] private QuickSlotUI sellingQuickSlotUI;

        private PlayerInput _playerInput;
        private QuickSlotUI _findSlotUI;

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            _playerInput = e.player.GetComponent<PlayerInput>();

            _playerInput.Actions.Slot1.started += context => { SelectQuickSlot(0); };
            _playerInput.Actions.Slot2.started += context => { SelectQuickSlot(1); };
            _playerInput.Actions.Slot3.started += context => { SelectQuickSlot(2); };
            _playerInput.Actions.Slot4.started += context => { SelectQuickSlot(3); };
            _playerInput.Actions.Slot5.started += context => { SelectQuickSlot(4); };
            _playerInput.Actions.Slot6.started += context => { SelectQuickSlot(5); };

            _playerInput.Actions.UseQuickSlot.started += context => { UseQuickSlot(); };
        }
        
        private void OnQuickSlotChanged(QuickSlotChangedEvent e)
        {
            foreach (var slotUI in quickSlotUIs)
            {
                slotUI.UpdateSlot();
            }
        }
        
        private void OnItemCountChanged(ItemCountChangedEvent e)
        {
            foreach (var slotUI in quickSlotUIs)
            {
                slotUI.UpdateSlot();
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
            Game.Get<GameManager>().player.inventory.UseQuickSlotItem(_selectedSlotIndex);
        }
        
        private void UpdateQuickSlot()
        {
            for (var i = 0; i < quickSlotUIs.Length; i++)
            {
                var slotUI = quickSlotUIs[i];
                
                if (i == _selectedSlotIndex)
                {
                    slotUI.selectedSlotImage.gameObject.SetActive(true);
                    slotUI.transform.SetAsLastSibling();
                }
                else
                {
                    quickSlotUIs[i].selectedSlotImage.gameObject.SetActive(false);
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
