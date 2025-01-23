using System;
using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuySlotUI : MonoBehaviour, ISlot
    {
        [Header("구매 슬롯 변수")]
        public Image itemImage;
        public Image slotImage;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemPriceText;
        public int slotID {get; private set;}
        public void SetSlotID(int id) { slotID = id; }
        
        public virtual void SetSlot(Slot targetSlot)
        {
            itemImage.sprite = targetSlot.GetSlotItem().sprite;
            itemNameText.text = targetSlot.GetSlotItem().name;
            itemPriceText.text = targetSlot.GetSlotItem().price.ToString();
        }

        public virtual SlotType GetSlotType() { return SlotType.Buy; }
        public virtual int GetSlotID() { return slotID; }
    }
}