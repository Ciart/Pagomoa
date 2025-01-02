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
            
            inventoryUI.choiceSlot = this;

            if (inventory.inventorySlots[inventoryUI.choiceSlot.id] == null)
                return;

            var item = inventory.inventorySlots[inventoryUI.choiceSlot.id].GetSlotItem();
                
            inventory.Add(item, 0);
            inventoryUI.UpdateSlots();
            inventory.RemoveArtifactData(item);
            inventoryUI.SetArtifactSlots();
        }
        
        // summary : SetItem 기능을 이어 받아 아이템 세팅에 이용되는 함수
        public override void SetSlot(Item setItem)
        {
            if (setItem.id != "")
            {
                itemImage.sprite = setItem.sprite;
                countText.text = GetSlotItemCount() == 0 ? "" : GetSlotItemCount().ToString();
            }
            else
            {
                itemImage.sprite = null;
                countText.text = "";
            } 
        }

        public void ResetSlot()
        {
            itemImage.sprite = null;
            countText.text = "";
        }
        
        private void Swap(ref InventorySlot a, ref InventorySlot b)
        {
            (a, b) = (b, a);
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            var inventorySlot = this;
            var targetSlot = eventData.pointerPress.GetComponent<InventorySlot>();
            var player = GameManager.instance.player;
            
            player.inventory.SwapSlot(id, targetSlot.id);
            Swap(ref inventorySlot, ref targetSlot);
        }
    }
}
