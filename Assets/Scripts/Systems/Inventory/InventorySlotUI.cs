using Ciart.Pagomoa.Entities.Players;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public enum SlotType
    {
        Default = 0,
        Inventory,
        Shop,
        Buy,
    }
    
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        public SlotType slotType = SlotType.Default; 

        public InventorySlot slot;
        public Image image;
        public TextMeshProUGUI count;
        public int id;

        public void ReleaseItem()
        {
            var inventory = UIManager.instance.bookUI.inventoryUI;
            
            inventory.choiceSlot = this;
            var player = GameManager.instance.player;

            if (player.inventory.items[inventory.choiceSlot.id].item == null)
                return;
            
            player.inventory.Add(player.inventory.items[inventory.choiceSlot.id].item, 0);
            inventory.UpdateSlots();
            player.inventory.RemoveArtifactData(player.inventory.items[inventory.choiceSlot.id].item);
            inventory.SetArtifactSlots();
        }
        public void SetItem(InventorySlot item)
        {
            if (item.item is null)
            {
                ResetItem();
                return;
            }
            
            SetSprite(item.item.itemImage);
            count.text = item.count == 0 ? "" : item.count.ToString();
        }
        
        public void ResetItem()
        {
            SetSprite(null);
            count.text = "";
        }
        
        private void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
            image.enabled = sprite != null;
        }
        public void OnDrop(PointerEventData eventData)
        {
            var player = GameManager.instance.player;
            
            player.inventory.SwapSlot(id, eventData.pointerPress.GetComponent<InventorySlotUI>().id);
            Swap(ref slot, ref eventData.pointerPress.GetComponent<InventorySlotUI>().slot);
        }

        private void Swap(ref InventorySlot a, ref InventorySlot b)
        {
            (a, b) = (b, a);
        }
    }
}
