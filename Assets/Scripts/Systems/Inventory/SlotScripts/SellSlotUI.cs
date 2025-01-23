using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SellSlotUI : MonoBehaviour, ISlot
    {
        [Header("판매 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI countText;
        public int slotID {get; private set;}
        public void SetSlotID(int id) { slotID = id; }
        public int GetSlotID() { return slotID; }

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
