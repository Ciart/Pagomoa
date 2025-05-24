using Ciart.Pagomoa.Systems;
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
        [SerializeField] private TextMeshProUGUI _itemCount;
        
        private RectTransform _rectTransform;
        
        public void UpdateItemInfo(int slotID)
        {
            var inventory = Game.Instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Inventory, slotID);
            
            if (targetSlot.GetSlotItemID() == "") return;
            _itemName.text = targetSlot.GetSlotItem().name;
            _itemInfo.text = targetSlot.GetSlotItem().description;
            _itemType.text = "";
            _itemCount.text = $"보유량 : {targetSlot.GetSlotItemCount().ToString()}";
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
