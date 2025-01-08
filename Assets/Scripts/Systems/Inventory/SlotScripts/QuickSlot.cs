using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Items;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlot : Slot
    {
        [Header("퀵슬롯 변수")]
        public Image slotImage;
        [SerializeField] public Image itemImage;
        
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private TextMeshProUGUI countText;
        
        public int id;
        public InventorySlot? referenceSlot; 

        private void Awake()
        {
            SetSlotType(SlotType.Quick);
            slotImage = GetComponent<Image>();
            countText.color = new Color(1, 1, 1, 255);
        }
        
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
                itemImage.sprite = _emptySprite;
                countText.text = "";
                GetSlotItem().ClearItemProperty();
                SetSlotItemCount(0);
            } 
        }
        
        public void ResetSlot()
        {
            var emptyItem = new Item();
            SetSlotItem(emptyItem);
            SetSlotItemCount(0);
            referenceSlot = null;
            
            itemImage.sprite = _emptySprite;
            countText.text = "";
        }
    }
}
