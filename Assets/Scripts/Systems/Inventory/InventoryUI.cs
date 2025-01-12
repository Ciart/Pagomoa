using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private RectTransform inventorySlotParent;
        [SerializeField] private InventorySlot InstanceInventorySlot;
        private List<InventorySlot> _inventoryUISlots = new List<InventorySlot>();
        public InventorySlot[] artifactSlotData = new InventorySlot[Inventory.MaxArtifactSlots];
        
        private void OnItemCountChanged(ItemCountChangedEvent e) { UpdateInventorySlot(); }

        public void InitInventorySlots() // slotData 갯수만큼 슬롯 만들기
        {
            for (int i = 0; i < Inventory.MaxSlots; i++)
            {
                var spawnedSlot = Instantiate(InstanceInventorySlot, inventorySlotParent.transform);
                spawnedSlot.InitSlot();
                _inventoryUISlots.Add(spawnedSlot);
                _inventoryUISlots[i].id = i;
                spawnedSlot.gameObject.SetActive(true);
            }
            GameManager.instance.player.inventory.InitSlots();
            UpdateInventorySlot();
        }
        
        public void SetArtifactSlots()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < artifactSlotData.Length; i++)
            {
                //artifactSlotData[i].SetSlot(inventory.artifactItems[i].s);
            }
        }
        
        public void UpdateInventorySlot() // List안의 Item 전체 인벤토리에 출력
        {
            var inventory = GameManager.instance.player.inventory;
            var quickSlotUI = UIManager.instance.quickSlotUI;
            
            var hasQuickSlot = false;
            
            for (var i = 0; i < Inventory.MaxSlots; i++)
            {
                _inventoryUISlots[i].SetSlot(inventory.inventoryItems[i]);
                if (_inventoryUISlots[i].referenceSlotID != -1)
                {
                    hasQuickSlot = true;   
                }
            }
            
            if (hasQuickSlot) quickSlotUI.UpdateQuickSlot();
        }

        public void UpdateInventorySlotByQuickSlotID(int quickSlotID)
        {
            var inventoryItems = GameManager.instance.player.inventory.inventoryItems;
            
            foreach (var slotUI in _inventoryUISlots)
            {
                if (slotUI.referenceSlotID != quickSlotID) continue;
                
                var itemCount = inventoryItems[slotUI.id].GetSlotItemCount() - 1;
                inventoryItems[slotUI.id].SetSlotItemCount(itemCount);
                
                if (itemCount == 0)
                    inventoryItems[slotUI.id].SetSlotItemID("");
            }
            
            UpdateInventorySlot();
        }
        
        public void SwapReferenceSlotID(int toChangeID, int beforeID, bool notSwap = false)
        {
            InventorySlot? beforeSlot = null;
            InventorySlot? toChangeSlot = null;
            
            foreach (var slot in _inventoryUISlots)
            {
                if (slot.referenceSlotID == toChangeID)
                    toChangeSlot = slot;
            }
            foreach (var slot in _inventoryUISlots)
            {
                if (slot.referenceSlotID == beforeID) 
                    beforeSlot = slot;
            }
            
            if (!beforeSlot) return;
            
            if (notSwap)
            {
                beforeSlot.referenceSlotID = toChangeID;
                return;
            }
            
            if (!toChangeSlot) return;

            (beforeSlot.referenceSlotID, toChangeSlot.referenceSlotID) = (toChangeID, beforeID);
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
            
            for (int i = 0; i < Inventory.MaxSlots; i++)
            {
                _inventoryUISlots[i].id = i;
            }
        }
        
        private void OnEnable()
        {
            UpdateInventorySlot();
            EventManager.AddListener<ItemCountChangedEvent>(OnItemCountChanged);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ItemCountChangedEvent>(OnItemCountChanged);
        }
    }
}
