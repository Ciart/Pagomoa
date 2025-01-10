using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public Slot slot { get; private set; }

        public void InitSlot() { slot = new Slot(); }
        
        [Header("인벤토리 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI countText;
        public int id = 0;
        public int referenceSlotID = 0;
        
        public ClickToSlot clickToSlot { get; private set; }

        private void Awake()
        {
            slot.SetSlotType(SlotType.Inventory);
            clickToSlot = GetComponent<ClickToSlot>();
        }

        public void ReleaseItem()
        {
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            var inventory = GameManager.instance.player.inventory;
            
            inventoryUI.chosenSlot = this;

            if (inventory.inventoryItems[inventoryUI.chosenSlot.id] == null) return;

            var item = inventory.inventoryItems[inventoryUI.chosenSlot.id].GetSlotItem();
                
            inventory.Add(item, 0);
            inventoryUI.UpdateSlots();
            inventory.RemoveArtifactData(item);
            inventoryUI.SetArtifactSlots();
        }
        
        // summary : SetItem 기능을 이어 받아 아이템 세팅에 이용되는 함수
        public void SetSlot(Slot targetSlot)
        {
            if (targetSlot.GetSlotItemID() == "")
            {
                ResetSlot();
                slot.SetSlotItemID("");
                slot.SetSlotItemCount(0);
            } 
            else if (targetSlot.GetSlotItemID() != "")
            {
                slot.SetSlotItemID(targetSlot.GetSlotItemID());
                slot.SetSlotItemCount(targetSlot.GetSlotItemCount());
                
                itemImage.sprite = targetSlot.GetSlotItem().sprite;
                countText.text = targetSlot.GetSlotItemCount() == 0 ? "" : targetSlot.GetSlotItemCount().ToString();
            }
        }

        public void ResetSlot()
        {
            itemImage.sprite = null;
            countText.text = "";
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            var inventory = GameManager.instance.player.inventory;
            var inventoryUI = UIManager.instance.bookUI.GetInventoryUI();
            eventData.pointerPress.TryGetComponent<InventorySlot>(out var dragSlot);
            
            if (!dragSlot || id == dragSlot.id) return;
            
            inventoryUI.SwapUISlot(id, dragSlot.id);
            inventory.SwapSlot(dragSlot.id, id);
            
            Debug.Log(inventory.inventoryItems[dragSlot.id].GetSlotItem() + " " + dragSlot.id);
        }
    }
}
