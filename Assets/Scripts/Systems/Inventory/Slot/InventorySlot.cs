using System;
using Ciart.Pagomoa.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventorySlot : Slot, IDropHandler
    {
        [Header("인벤토리 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI countText;
        public int id;
        
        public ClickToSlot clickToSlot { get; private set; }

        private void Awake()
        {
            SetSlotType(SlotType.Inventory);
            clickToSlot = GetComponent<ClickToSlot>();
        }

        public void ReleaseItem()
        {
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var inventory = GameManager.instance.player.inventory;
            
            inventoryUI.chosenSlot = this;

            if (inventory.inventorySlots[inventoryUI.chosenSlot.id] == null)
                return;

            var item = inventory.inventorySlots[inventoryUI.chosenSlot.id].GetSlotItem();
                
            inventory.Add(item, 0);
            inventoryUI.UpdateSlots();
            inventory.RemoveArtifactData(item);
            inventoryUI.SetArtifactSlots();
        }
        
        // summary : SetItem 기능을 이어 받아 아이템 세팅에 이용되는 함수
        public override void SetSlot(Slot slot)
        {
            if (slot.GetSlotItem().id != "")
            {
                SetSlotItem(slot.GetSlotItem());
                SetSlotItemCount(slot.GetSlotItemCount());
                itemImage.sprite = slot.GetSlotItem().sprite;
                countText.text = GetSlotItemCount() == 0 ? "" : GetSlotItemCount().ToString();
            }
            else
            {
                itemImage.sprite = null;
                countText.text = "";
                GetSlotItem().ClearItemProperty();
                SetSlotItemCount(0);
            } 
        }

        public void ResetSlot()
        {
            itemImage.sprite = null;
            countText.text = "";
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            eventData.pointerPress.TryGetComponent<InventorySlot>(out var dragSlot);
            
            if (id == dragSlot.id || !dragSlot) return;
            
            inventoryUI.SwapUISlot(id, dragSlot.id);
            inventory.SwapSlot(dragSlot.id, id);
        }
    }
}
