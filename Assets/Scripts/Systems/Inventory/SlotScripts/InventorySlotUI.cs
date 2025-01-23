using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventorySlotUI : MonoBehaviour, ISlot
    {
        [Header("인벤토리 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI countText;
        public int slotID {get; private set;}
        public void SetSlotID(int id) { slotID = id; }
        public SlotMenu slotMenu { get; private set; }

        private void Awake()
        {
            slotMenu = GetComponent<SlotMenu>();
            itemImage = GetComponentInChildren<Image>();
            countText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public SlotType GetSlotType() { return SlotType.Inventory; }
        public int GetSlotID() { return slotID; }

        public void SetSlot(Slot targetSlot)
        {
            if (targetSlot.GetSlotItemID() == "")
            {
                itemImage.sprite = null;
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
