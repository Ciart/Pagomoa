using System;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Players;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlotItemDB : MonoBehaviour
    {

        static public QuickSlotItemDB instance;

        [SerializeField] public List<InventoryItem> quickSlotItems = new List<InventoryItem>();
        [SerializeField] public List<QuickSlot> quickSlots = new List<QuickSlot>();
        [SerializeField] public QuickSlot selectedSlot;

        private PlayerInput playerInput;
        [SerializeField] private QuickSlot _sellingQuickSlot;

        private void Start()
        {
            instance = this;
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            playerInput = e.player.GetComponent<PlayerInput>();

            playerInput.Actions.Slot1.started += context => { ControlQuickSlot(0); };
            playerInput.Actions.Slot2.started += context => { ControlQuickSlot(1); };
            playerInput.Actions.Slot3.started += context => { ControlQuickSlot(2); };
            playerInput.Actions.Slot4.started += context => { ControlQuickSlot(3); };
            playerInput.Actions.Slot5.started += context => { ControlQuickSlot(4); };
            playerInput.Actions.Slot6.started += context => { ControlQuickSlot(5); };

            playerInput.Actions.UseQuickSlot.started += context => { UseQuickSlot(); };
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        public void SetCount(Item data)
        {
            var inventoryItem = InventoryDB.Instance.items.Find(inventoryItem => inventoryItem.item == data);
            var quickslotItem = quickSlotItems.Find(quickslotItem => quickslotItem.item == data);

            if (quickslotItem != null && inventoryItem.count != 0)
            {
                quickslotItem.count = inventoryItem.count;
                for (int i = 0; i < quickSlots.Count; i++)
                {
                    if (quickSlots[i].inventoryItem == quickslotItem && quickslotItem.count != 0)
                    {
                        _sellingQuickSlot = quickSlots[i];
                        quickSlots[i].itemCount.text = quickslotItem.count.ToString();
                    }
                }
            }
            else if (quickslotItem == null)
                return;
        }
        public void CleanSlot(Item data)
        {
            var quickslotItem = quickSlotItems.Find(quickslotItem => quickslotItem.item == data);
            var quickslot = quickSlots.Find(quickSlots => quickSlots.inventoryItem.item == data);
            if (quickslotItem != null)
            {
                for (int i = 0; i < quickSlotItems.Count; i++)
                {
                    if (quickSlotItems[i].item == quickslotItem.item)
                    {
                        quickSlotItems[i] = null;
                        quickslot.SetSlotNull();
                        if (_sellingQuickSlot != null)
                            _sellingQuickSlot.SetSlotNull();
                    }
                }
            }
            else
                return;
        }
        private void UseQuickSlot()
        {
            if (selectedSlot == null || selectedSlot.inventoryItem == null || selectedSlot.inventoryItem.item == null)
                return;

            if (selectedSlot.inventoryItem.item.itemType == Item.ItemType.Use)
            {
                selectedSlot.inventoryItem.count -= 1;
                selectedSlot.SetItemCount();
                selectedSlot.UseItem();
                if (selectedSlot.inventoryItem.count == 0)
                    InventoryDB.Instance.Use(selectedSlot.inventoryItem.item);
                if (Inventory.Instance)
                {
                    Inventory.Instance.DeleteSlot();
                    Inventory.Instance.UpdateSlot();
                }
                else
                    return;
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
            for (int index = 0; index < quickSlots.Count; index++)
            {
                if (n == index)
                {
                    if (selectedSlot != quickSlots[index])
                    {
                        selectedSlot = quickSlots[index];
                        quickSlots[index].selectedSlotImage.gameObject.SetActive(true);
                        selectedSlot.transform.SetAsLastSibling();
                    }
                    else
                    {
                        quickSlots[index].selectedSlotImage.gameObject.SetActive(false);
                        selectedSlot = null;
                    }
                }
                else
                    quickSlots[index].selectedSlotImage.gameObject.SetActive(false);
            }
        }
    }
}
