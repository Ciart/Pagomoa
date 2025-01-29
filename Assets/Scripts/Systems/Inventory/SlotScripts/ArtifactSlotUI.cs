using Ciart.Pagomoa.Systems.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class ArtifactSlotUI : MonoBehaviour, ISlot
    {
        [Header("자식 변수")] 
        [SerializeField] private Sprite _emptySprite;
        public Image itemImage;

        private int _slotID;
        public int GetSlotID() { return _slotID; }
        public void SetSlotID(int slotID) { _slotID = slotID; }

        public SlotType GetSlotType() { return SlotType.Artifact; }

        public void SetSlot(Slot slot)
        {
            if (slot.GetSlotItemID() == "")
            {
                itemImage.sprite = _emptySprite;
            }
            else
            {
                itemImage.sprite = slot.GetSlotItem().sprite;
            }
        }
    }
}
