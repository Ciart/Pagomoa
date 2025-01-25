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
        public Image slotImage;
        public Button buyCheckButton;
        
        [Header("자식 변수")]
        public Image itemImage;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemPriceText;
        private int _slotID;
        public void SetSlotID(int id) { _slotID = id; }
        public virtual int GetSlotID() { return _slotID; }
        private void Start()
        {
            buyCheckButton.onClick.AddListener(
                () => UIManager.instance.shopUI.BuyCheck(this)
            );
            
            slotImage = GetComponent<Image>();
            buyCheckButton = GetComponent<Button>();
        }
        
        public void SetSlot(Slot targetSlot)
        {
            itemImage.sprite = targetSlot.GetSlotItem().sprite;
            itemNameText.text = targetSlot.GetSlotItem().name;
            itemPriceText.text = targetSlot.GetSlotItem().price.ToString();
        }

        public virtual SlotType GetSlotType() { return SlotType.Buy; }
    }
}