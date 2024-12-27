using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuyArtifactSlot : BuySlot
    {
        [SerializeField] public GameObject soldOut;
        [SerializeField] public TextMeshProUGUI itemNum;
    
        public void UpdateArtifactSlot()
        {
            image.sprite = slot.item.sprite;
            itemName.text = slot.item.name;
            itemPrice.text = slot.item.price.ToString();
            itemNum.text = slot.count.ToString();
        }
    }
}
