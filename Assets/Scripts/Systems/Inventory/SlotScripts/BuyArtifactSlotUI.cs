using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    [RequireComponent(typeof(Button))]
    public class BuyArtifactSlotUI : BuySlotUI
    {
        [Header("아티팩트 슬롯 변수")]
        public TextMeshProUGUI artifactCount;
        public Button artifactSlotButton;
        public Image soldOutImage;

        public void Start()
        {
            slotImage = GetComponent<Image>();
            soldOutImage = GetComponent<Image>();
            artifactSlotButton = GetComponent<Button>();
            artifactCount = GetComponent<TextMeshProUGUI>();
        }

        public override void SetSlot(Slot targetSlot)
        {
            artifactCount.text = targetSlot.GetSlotItemCount().ToString();
        }
        
        public override SlotType GetSlotType() { return SlotType.BuyArtifact; }
    }
}
