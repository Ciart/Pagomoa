using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SellSlotUI : MonoBehaviour, ISlot
    {
        [Header("판매 슬롯 변수")]
        public Image itemImage;
        public Button sellCheckButton;
        public TextMeshProUGUI countText;
        private int _slotID;
        public void SetSlotID(int id) { _slotID = id; }
        public int GetSlotID() { return _slotID; }

        public void AddSlotEvent()
        {
            sellCheckButton.onClick.AddListener(() =>
                UIManager.instance.shopUI.SellCheck(this)
            );
        }

        public void SetSlot(Slot targetSlot)
        {
            if (targetSlot.GetSlotItemID() == "")
            {
                itemImage.sprite = null;
                countText.text = "";
            }
            else if (targetSlot.GetSlotItem().id != "")
            {
                itemImage.sprite = targetSlot.GetSlotItem().sprite;
                countText.text = targetSlot.GetSlotItemCount() == 0 ? "" : targetSlot.GetSlotItemCount().ToString();
            }
        }

        public SlotType GetSlotType() { return SlotType.Sell; }
    }
}
