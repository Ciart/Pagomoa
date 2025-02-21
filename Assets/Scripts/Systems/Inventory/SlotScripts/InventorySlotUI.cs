using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventorySlotUI : MonoBehaviour, ISlot
    {
        [Header("자식 인벤토리 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI countText;
        private int _slotID;
        public int GetSlotID() { return _slotID; }
        public void SetSlotID(int id) { _slotID = id; }
        public SlotType GetSlotType() { return SlotType.Inventory; }
        public SlotMenu slotMenu { get; private set; }

        private void Start()
        {
            slotMenu = GetComponent<SlotMenu>();
        }

        public void SetSlot(Slot targetSlot)
        {
            if (targetSlot.GetSlotItemID() == "")
            {
                var color = itemImage.color;
                color.a = 0.0f;
                itemImage.color = color;
                itemImage.sprite = null;
                countText.text = "";
            } 
            else if (targetSlot.GetSlotItemID() != "")
            {
                var color = itemImage.color;
                color.a = 255.0f;
                itemImage.color = color;
                itemImage.sprite = targetSlot.GetSlotItem().sprite;
                countText.text = targetSlot.GetSlotItemCount() == 0 ? "" : targetSlot.GetSlotItemCount().ToString();
            }
        }
    }
}
