using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] public InventorySlot choiceSlot;
        [SerializeField] public InventorySlot hoverSlot;
        [SerializeField] private GameObject slotParent;
        [SerializeField] private GameObject slot;
        [SerializeField] private Sprite emptyImage;
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();
        
        private const int MaxArtifactSlotData = 4;
        public InventorySlot[] artifactSlotData = new InventorySlot[MaxArtifactSlotData];

        private void Awake()
        {
            // QuickSlotContainerUI.instance.transform.SetAsLastSibling();
        }

        private void OnItemCountChanged(ItemCountChangedEvent e) { UpdateSlots(); }

        private void OnEnable()
        {
            UpdateSlots();
            
            EventManager.AddListener<ItemCountChangedEvent>(OnItemCountChanged);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ItemCountChangedEvent>(OnItemCountChanged);
        }

        public void MakeSlots() // slotdatas 갯수만큼 슬롯 만들기
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < inventory.items.Length; i++)
            {
                GameObject spawnedSlot = Instantiate(slot, slotParent.transform);
                inventory.items[i] = spawnedSlot.GetComponent<InventorySlot>();
                inventorySlots.Add(spawnedSlot.GetComponent<InventorySlot>());
                inventorySlots[i].id = i;
                spawnedSlot.SetActive(true);
            }
            UpdateSlots();
        }
        
        public void UpdateSlots() // List안의 Item 전체 인벤토리에 출력
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (var i = 0; i < inventorySlots.Count; i++)
            {
                if (inventory.items[i] == null)
                    inventorySlots[i].GetSlotItem().ClearItemProperty();
                else
                    inventorySlots[i].SetSlot(inventory.items[i].GetSlotItem());
            }
        }
        
        public void ResetSlots() // 인벤토리에 출력된 아이템들 전부 NULL
        {
            foreach (var slot in inventorySlots)
                slot.ResetSlot();

        }
        
        public void SetArtifactSlots()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < artifactSlotData.Length; i++)
            {
                artifactSlotData[i].SetSlotItem(inventory.artifactItems[i].GetSlotItem());
                artifactSlotData[i].SetSlot(artifactSlotData[i].GetSlotItem());
            }
        }
    }
}
