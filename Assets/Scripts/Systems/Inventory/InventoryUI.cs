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
        public InventorySlot choiceSlot;
        public InventorySlot hoverSlot;
        [SerializeField] private RectTransform inventorySlotParent;
        [SerializeField] private InventorySlot inventorySlot;
        [SerializeField] private Sprite emptyImage;
        private List<InventorySlot> _inventorySlots = new List<InventorySlot>();
        
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

        public void MakeSlots() // slotData 갯수만큼 슬롯 만들기
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < inventory.inventorySlots.Length; i++)
            {
                var spawnedSlot = Instantiate(inventorySlot, inventorySlotParent.transform);
                inventory.inventorySlots[i] = spawnedSlot.GetComponent<InventorySlot>();
                _inventorySlots.Add(inventory.inventorySlots[i]);
                _inventorySlots[i].id = i;
                spawnedSlot.gameObject.SetActive(true);
            }
            UpdateSlots();
        }
        
        public void UpdateSlots() // List안의 Item 전체 인벤토리에 출력
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (var i = 0; i < _inventorySlots.Count; i++)
            {
                if (inventory.inventorySlots[i] == null)
                    _inventorySlots[i].GetSlotItem().ClearItemProperty();
                else
                    _inventorySlots[i].SetSlot(inventory.inventorySlots[i].GetSlotItem());
            }
            
        }
        
        public void ResetSlots() // 인벤토리에 출력된 아이템들 전부 NULL
        {
            foreach (var slot in _inventorySlots)
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
