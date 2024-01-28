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
            image.sprite = inventoryItem.item.itemImage;
            itemName.text = inventoryItem.item.itemName;
            itemPrice.text = inventoryItem.item.itemPrice.ToString();
            itemNum.text = inventoryItem.count.ToString();
        }
    }
}
