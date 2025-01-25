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
        public Image soldOutImage;

        public void Start()
        {
            buyCheckButton.onClick.AddListener(
                () => UIManager.instance.shopUI.BuyCheck(this)
                );
            
            slotImage = GetComponent<Image>();
        }
        
        public override SlotType GetSlotType() { return SlotType.BuyArtifact; }
    }
}
