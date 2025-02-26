using TMPro;
using UnityEngine;
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

        private int _slotID;
        public void SetSlotID(int id) { _slotID = id; }
        public SlotType GetSlotType() { return SlotType.Quick; }
        public int GetSlotID() { return _slotID; }
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
