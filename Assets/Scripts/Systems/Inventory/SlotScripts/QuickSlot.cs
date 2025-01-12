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
    public class QuickSlot : MonoBehaviour
    {
        public Slot slot { get; private set; }
        [Header("퀵슬롯 변수")]
        public Image slotImage;
        [SerializeField] public Image itemImage;
        
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private TextMeshProUGUI countText;

        public int id = 0;
         

        private void Awake()
        {
            slot = new Slot(); 
            slot.SetSlotType(SlotType.Quick);
            slotImage = GetComponent<Image>();
            countText.color = new Color(1, 1, 1, 255);
        }
        
        public void SetSlot(Slot targetSlot)
        {
            if (targetSlot.GetSlotItemID() == "")
            {
                ResetSlot();
            } 
            else if (targetSlot.GetSlotItemID() != "")
            {
                slot.SetSlotItemID(targetSlot.GetSlotItem().id);
                slot.SetSlotItemCount(targetSlot.GetSlotItemCount());
                
                itemImage.sprite = targetSlot.GetSlotItem().sprite;
                countText.text = targetSlot.GetSlotItemCount() == 0 ? "" : targetSlot.GetSlotItemCount().ToString();
            }
        }
        
        public void ResetSlot()
        {
            slot.SetSlotItemID("");
            slot.SetSlotItemCount(0);
            
            itemImage.sprite = _emptySprite;
            countText.text = "";
        }
    }
}
