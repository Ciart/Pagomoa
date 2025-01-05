using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    [RequireComponent(typeof(Button))]
    public class BuyArtifactSlot : BuySlot
    {
        public TextMeshProUGUI artifactCount;
        public Button artifactSlotButton;
        
        public GameObject soldOut;
        
        private void Awake()
        {
            SetSlotType(SlotType.BuyArtifact);
            artifactSlotButton = GetComponent<Button>();
            SetCountBuySlot();
        }
        
        public override void UpdateBuySlot()
        {
            itemImage.sprite = GetSlotItem().sprite;
            itemNameText.text = GetSlotItem().name;
            itemPriceText.text = GetSlotItem().price.ToString();
            artifactCount.text = GetSlotItemCount().ToString();
        }
    }
}
