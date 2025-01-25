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
    public class QuickSlotUI : MonoBehaviour, ISlot
    {
        [Header("퀵슬롯 변수")]
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Image slotImage;
        
        [Header("자식 변수")]
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI countText;
        
        public int slotID {get; private set;}
        public void SetSlotID(int id) { slotID = id; }
        public SlotType GetSlotType() { return SlotType.Quick; }
        public int GetSlotID() { return slotID; }
        public void SetSlotImage(Sprite sprite) { slotImage.sprite = sprite; }
        
        private void Awake()
        {
            slotImage = GetComponent<Image>();
            countText.color = new Color(1, 1, 1, 255);
        }
        
        public void SetSlot(Slot targetSlot)
        {
            if (targetSlot.GetSlotItemID() == "")
            {
                itemImage.sprite = _emptySprite;
                countText.text = "";
            } 
            else if (targetSlot.GetSlotItemID() != "")
            {
                itemImage.sprite = targetSlot.GetSlotItem().sprite;
                countText.text = targetSlot.GetSlotItemCount() == 0 ? "" : targetSlot.GetSlotItemCount().ToString();
            }
        }

        
    }
}
