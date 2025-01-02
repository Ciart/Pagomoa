using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerEnter.TryGetComponent(out InventorySlot slot))
            {
                if (slot.GetSlotItem().id == "") return;
                
                var hover = UIManager.instance.bookUI.GetHoverItemInfo();
                
                var newPosition = new Vector2(
                    slot.transform.position.x + hover.GetPositionOffSet().x
                    , slot.transform.position.y - hover.GetPositionOffSet().y);
                
                hover.gameObject.SetActive(true);
                hover.transform.position = newPosition;
                hover.UpdateItemInfo(slot);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            var hover = UIManager.instance.bookUI.GetHoverItemInfo();
            
            hover.OffItemInfo();
        }
    }
}
