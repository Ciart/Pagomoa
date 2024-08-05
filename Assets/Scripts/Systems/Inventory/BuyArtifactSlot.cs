using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuyArtifactSlot : BuySlot
    {
        [SerializeField] public GameObject _soldOut;
        [SerializeField] public TextMeshProUGUI itemNum;
    
        public void UpdateArtifactSlot()
        {
            image.sprite = slot.item.itemImage;
            itemName.text = slot.item.itemName;
            itemPrice.text = slot.item.itemPrice.ToString();
            itemNum.text = slot.count.ToString();
        }
    }
}
