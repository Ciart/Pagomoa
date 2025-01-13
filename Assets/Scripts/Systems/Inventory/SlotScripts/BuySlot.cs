using System;
using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlot : MonoBehaviour, ISlot
    {
        public Slot slot { get; private set; } = new Slot();
        
        [Header("구매 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemPriceText;

        private void Awake()
        {
            slot.SetSlotType(SlotType.Buy);
            SetCountBuySlot();
        }
        
        public virtual void SetSlot(Slot targetSlot)
        {
            itemImage.sprite = slot.GetSlotItem().sprite;
            itemNameText.text = slot.GetSlotItem().name;
            itemPriceText.text = slot.GetSlotItem().price.ToString();
        }
        protected void SetCountBuySlot()
        {
            if (slot.GetSlotType() == SlotType.Buy)
                slot.SetSlotItemCount(100000);
            else if (slot.GetSlotType() == SlotType.BuyArtifact)
                slot.SetSlotItemCount(1);
        }

        public SlotType GetSlotType() { return slot.GetSlotType(); }

        public int GetSlotID() { throw new NotImplementedException(); }

        public void ResetSlot() { throw new NotImplementedException(); }
    }
}