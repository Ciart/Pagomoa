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
        public InventorySlot chosenSlot;
        public InventorySlot hoverSlot;
        [SerializeField] private RectTransform inventorySlotParent;
        [SerializeField] private InventorySlot inventorySlot;
        [SerializeField] private Sprite emptyImage;
        private List<InventorySlot> _inventoryUISlots = new List<InventorySlot>();
        
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
                _inventoryUISlots.Add(inventory.inventorySlots[i]);
                _inventoryUISlots[i].id = i;
                spawnedSlot.gameObject.SetActive(true);
            }
            UpdateSlots();
        }
        
        public void UpdateSlots() // List안의 Item 전체 인벤토리에 출력
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (var i = 0; i < Inventory.MaxSlots; i++)
            {
                if (inventory.inventorySlots[i].GetSlotItem().id == "")
                    _inventoryUISlots[i].GetSlotItem().ClearItemProperty();
                else
                {
                    _inventoryUISlots[i].SetSlot(inventory.inventorySlots[i]);
                }
            }
        }
        
        public void ResetInventoryUI() // 인벤토리에 출력된 아이템들 전부 NULL
        {
            foreach (var slot in _inventoryUISlots)
                slot.ResetSlot();
        }
        
        public void SetArtifactSlots()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < artifactSlotData.Length; i++)
            {
                artifactSlotData[i].SetSlot(inventory.artifactItems[i]);
            }
        }

        public void SwapUISlot(int dropID, int targetID)
        {
            _inventoryUISlots[targetID].transform.SetSiblingIndex(dropID + 1); 
            _inventoryUISlots[dropID].transform.SetSiblingIndex(targetID + 1);
        }
    }
}
