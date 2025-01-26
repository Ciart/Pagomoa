using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa
{
    public class ClickArtifactSlot : MonoBehaviour
        , IPointerClickHandler , IDropHandler
    {
        private const float ClickTime = 0.3f;
        private const int DoubleClick = 2;

        private ArtifactSlotUI _artifactSlotUI => GetComponent<ArtifactSlotUI>();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData is { clickCount: DoubleClick, clickTime: < ClickTime })
            {
                Debug.Log("더블 클릭");
                // 더블 클릭
                // 아티팩트 장착 해제   
                
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                // 우클릭 
                // 아티팩트 
            }
            
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            eventData.pointerDrag.TryGetComponent<InventorySlotUI>(out var inventorySlot);

            if (inventorySlot)
            {
                var inventory = GameManager.instance.player.inventory;
                inventory.EquipArtifact(inventorySlot);
            }
        }
    }
}
