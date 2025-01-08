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
        
        [SerializeField] private RectTransform inventorySlotParent;
        [SerializeField] private InventorySlot InstanceInventorySlot;
        private List<InventorySlot> _inventoryUISlots = new List<InventorySlot>();
        
        private const int MaxArtifactSlotData = 4;
        public InventorySlot[] artifactSlotData = new InventorySlot[MaxArtifactSlotData];
        
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

        public void InitInventorySlots() // slotData 갯수만큼 슬롯 만들기
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < Inventory.MaxSlots; i++)
            {
                var spawnedSlot = Instantiate(InstanceInventorySlot, inventorySlotParent.transform);
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
        
        public void SetArtifactSlots()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < artifactSlotData.Length; i++)
            {
                artifactSlotData[i].SetSlot(inventory.artifactItems[i]);
            }
        }

        public void SwapUISlot(int targetID, int dragID)
        {
            var targetIndex = _inventoryUISlots[targetID].transform.GetSiblingIndex();
            var dragIndex = _inventoryUISlots[dragID].transform.GetSiblingIndex();
            
            Debug.Log("Want to drop in  " + targetIndex);
            if (targetIndex == Inventory.MaxSlots)
            {
                Debug.Log("You drop in Last Sibling");
                _inventoryUISlots[dragID].transform.SetAsLastSibling();
                _inventoryUISlots[targetID].transform.SetSiblingIndex(dragID + 1);
            }
            else if (dragIndex == Inventory.MaxSlots)
            {
                Debug.Log("You hold Last Sibling");
                _inventoryUISlots[dragID].transform.SetSiblingIndex(targetID + 1);
                _inventoryUISlots[targetID].transform.SetAsLastSibling();
            }
            else
            {
                Debug.Log("Normal Swap");
                _inventoryUISlots[dragID].transform.SetSiblingIndex(targetID + 1); 
                _inventoryUISlots[targetID].transform.SetSiblingIndex(dragID + 1);   
            }

            (_inventoryUISlots[dragID], _inventoryUISlots[targetID]) =
                (_inventoryUISlots[targetID], _inventoryUISlots[dragID]);  
        }
    }
}
