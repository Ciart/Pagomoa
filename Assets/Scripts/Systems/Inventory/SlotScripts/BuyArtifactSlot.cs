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
            buySlot.SetSlotType(SlotType.BuyArtifact);
            artifactSlotButton = GetComponent<Button>();
            SetCountBuySlot();
        }
        
        public override void UpdateBuySlot()
        {
            itemImage.sprite = buySlot.GetSlotItem().sprite;
            itemNameText.text = buySlot.GetSlotItem().name;
            itemPriceText.text = buySlot.GetSlotItem().price.ToString();
            artifactCount.text = buySlot.GetSlotItemCount().ToString();
        }
    }
}
