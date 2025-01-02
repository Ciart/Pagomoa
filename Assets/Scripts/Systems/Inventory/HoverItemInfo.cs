using Ciart.Pagomoa.Systems.Inventory;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Ciart.Pagomoa
{
    public class HoverItemInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemType;
        [SerializeField] private TextMeshProUGUI _itemInfo;
        
        private RectTransform _rectTransform;
        
        public void UpdateItemInfo(InventorySlot slot)
        {
            _itemName.text = slot.GetSlotItem().name;
            _itemInfo.text = slot.GetSlotItem().description;
            _itemType.text = slot.GetSlotItem().type.ToString();
        }

        public Vector2 GetPositionOffSet()
        {
            _rectTransform = _rectTransform ?? GetComponent<RectTransform>();   
            
            var width = _rectTransform.sizeDelta.x / 2;
            var height = _rectTransform.sizeDelta.y / 2;
            
            return new Vector2(width, height);
        }
        
        
        public void OffItemInfo() { gameObject.SetActive(false); }
    }
}
