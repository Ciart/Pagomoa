using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlotUI : MonoBehaviour
    {

        static public QuickSlotUI instance;
        
        const int QuickSlots = 6;
        public QuickSlot[] quickSlotsUI = new QuickSlot[QuickSlots];
        [SerializeField] public QuickSlot selectedSlot;
        
        [SerializeField] private QuickSlot _sellingQuickSlot;

        private PlayerInput _playerInput;
        private QuickSlot _findSlot;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            _playerInput = e.player.GetComponent<PlayerInput>();

            _playerInput.Actions.Slot1.started += context => { ControlQuickSlot(0); };
            _playerInput.Actions.Slot2.started += context => { ControlQuickSlot(1); };
            _playerInput.Actions.Slot3.started += context => { ControlQuickSlot(2); };
            _playerInput.Actions.Slot4.started += context => { ControlQuickSlot(3); };
            _playerInput.Actions.Slot5.started += context => { ControlQuickSlot(4); };
            _playerInput.Actions.Slot6.started += context => { ControlQuickSlot(5); };

            _playerInput.Actions.UseQuickSlot.started += context => { UseQuickSlot(); };
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        public void SetQuickSlotItemCount(Item data)
        {
            int itemsIdx = Array.FindIndex(GameManager.player.inventoryDB.items, element => element.item == data);
            int quickSlotsIndex = Array.FindIndex(GameManager.player.inventoryDB.quickSlots, element => element.item == data);
            
            if (itemsIdx != -1  && quickSlotsIndex != -1)
            {
                InventoryItem item = GameManager.player.inventoryDB.items[itemsIdx];
                InventoryItem quickSlotItem = GameManager.player.inventoryDB.quickSlots[quickSlotsIndex];
                
                for (int i = 0; i < GameManager.player.inventoryDB.quickSlots.Length; i++)
                {
                    if (quickSlotsIndex == quickSlotsUI[i].id)
                    {
                        _findSlot = quickSlotsUI[i];
                    }
                }
                
                if (item.count > 1)
                {
                    _findSlot.itemCount.text = (item.count - 1).ToString();
                }
                else if (item.count == 1)
                {
                    quickSlotItem.item = null;
                    quickSlotItem.count = 0;
                    _findSlot.itemImage.sprite = _findSlot.transparentImage;
                    _findSlot.itemCount.text = null;
                }
            }
        }
        private void UseQuickSlot()
        {
            if (selectedSlot == null || selectedSlot.inventoryItem == null || selectedSlot.inventoryItem.item == null)
                return;

            if (selectedSlot.inventoryItem.item.itemType == Item.ItemType.Use)
            {
                
                selectedSlot.UseItem();
                SetQuickSlotItemCount(selectedSlot.inventoryItem.item);
                
                GameManager.player.inventoryDB.DecreaseItemCount(selectedSlot.inventoryItem.item);
                Inventory.Instance.DeleteSlot();
                Inventory.Instance.UpdateSlot();
               
            }
            else if (selectedSlot.inventoryItem.item.itemType == Item.ItemType.Inherent)
            {
                selectedSlot.UseItem();
            }
            else
                return;
        }
        public void ControlQuickSlot(int n)
        {
            for (int index = 0; index < quickSlotsUI.Length; index++)
            {
                if (n == index)
                {
                    if (selectedSlot != quickSlotsUI[index])
                    {
                        selectedSlot = quickSlotsUI[index];
                        quickSlotsUI[index].selectedSlotImage.gameObject.SetActive(true);
                        selectedSlot.transform.SetAsLastSibling();
                    }
                    else
                    {
                        quickSlotsUI[index].selectedSlotImage.gameObject.SetActive(false);
                        selectedSlot = null;
                    }
                }
                else
                    quickSlotsUI[index].selectedSlotImage.gameObject.SetActive(false);
            }
        }
    }
}
