using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    [RequireComponent(typeof(Button))]
    public class BuyArtifactSlot : BuySlot
    {
        [Header("아티팩트 슬롯 변수")]
        public TextMeshProUGUI artifactCount;
        public Button artifactSlotButton;
        
        public GameObject soldOut;
        
        private void Awake()
        {
            slot.SetSlotType(SlotType.BuyArtifact);
            artifactSlotButton = GetComponent<Button>();
            SetCountBuySlot();
        }
        
        public override void SetSlot(Slot targetSlot)
        {
            itemImage.sprite = slot.GetSlotItem().sprite;
            itemNameText.text = slot.GetSlotItem().name;
            itemPriceText.text = slot.GetSlotItem().price.ToString();
            artifactCount.text = slot.GetSlotItemCount().ToString();
        }
    }
}
