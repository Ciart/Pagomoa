using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Sprite[] hoverImage;
        [SerializeField] private Image boostImage;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            boostImage.sprite = hoverImage[0];
            
            
            
            if (eventData.pointerEnter.TryGetComponent(out InventorySlotUI inventorySlot))
            {
                var slot = Game.Instance.player.inventory.FindSlot(SlotType.Inventory, inventorySlot.GetSlotID());
                if (slot.GetSlotItemID() == "") return;
                
                var hover = UIManager.instance.bookUI.GetHoverItemInfo();
                
                var newPosition = new Vector2(
                    inventorySlot.transform.position.x + hover.GetPositionOffSet().x
                    , inventorySlot.transform.position.y - hover.GetPositionOffSet().y);
                
                hover.gameObject.SetActive(true);
                hover.transform.position = newPosition;
                hover.UpdateItemInfo(inventorySlot.GetSlotID());
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            boostImage.sprite = hoverImage[1];
            
            var hover = UIManager.instance.bookUI.GetHoverItemInfo();
            
            hover.OffItemInfo();
        }
        
        private void OnEnable()
        {
            boostImage.sprite = hoverImage[1];
        }
    }
}
