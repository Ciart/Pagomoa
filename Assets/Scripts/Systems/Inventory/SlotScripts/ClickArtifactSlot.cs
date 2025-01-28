using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa
{
    public class ClickArtifactSlot : MonoBehaviour
        , IPointerClickHandler , IDropHandler, IBeginDragHandler,  IDragHandler, IEndDragHandler
    {
        private const float ClickTime = 0.3f;
        private const int DoubleClick = 2;
        private float _firstClickTime = 0.0f;

        private ArtifactSlotUI _artifactSlotUI => GetComponent<ArtifactSlotUI>();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
            var artifact =inventory.FindSlot(SlotType.Artifact, _artifactSlotUI.GetSlotID());
            
            if (eventData.clickCount == 1)
            {
                _firstClickTime = eventData.clickTime;
            }
            if (eventData.clickCount == DoubleClick 
                && eventData.clickTime - _firstClickTime <= ClickTime)
            {
                inventory.UnEquipArtifact(_artifactSlotUI);
                _firstClickTime = 0.0f;
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (artifact.GetSlotItemID() == "") return;
                
                var rightClickMenu = UIManager.instance.bookUI.GetRightClickMenu();
                rightClickMenu.DeleteMenu();
                rightClickMenu.transform.position = _artifactSlotUI.transform.position;
                rightClickMenu.SetClickedSlot(this);
                rightClickMenu.ArtifactMenu();
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
            var targetSlot = inventory.FindSlot(SlotType.Artifact, _artifactSlotUI.GetSlotID());
            
            if (targetSlot == null) return;
            if (targetSlot.GetSlotItemID() == "") return;
            
            DragItem.instance.DragSetImage(targetSlot.GetSlotItem().sprite);
            DragItem.instance.transform.position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData) { DragItem.instance.transform.position = eventData.position; }
        public void OnEndDrag(PointerEventData eventData) { DragItem.instance.SetColor(0); }
        
        public void OnDrop(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
            
            eventData.pointerDrag.TryGetComponent<InventorySlotUI>(out var inventorySlotUI);
            if (inventorySlotUI)
            {
                inventory.EquipDraggedArtifact(inventorySlotUI, _artifactSlotUI);
                return;
            }
            
            eventData.pointerDrag.TryGetComponent<ArtifactSlotUI>(out var artifactSlotUI);
            if (artifactSlotUI)
            {
                inventory.SwapArtifact(artifactSlotUI, _artifactSlotUI);
            }
        }

        public void RightClickMenu()
        {
            var inventory = GameManager.instance.player.inventory;
            inventory.UnEquipArtifact(_artifactSlotUI);
        }
    }
}
