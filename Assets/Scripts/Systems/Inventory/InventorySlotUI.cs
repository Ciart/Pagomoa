using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        public InventorySlot slot;
        public Image image;
        public TextMeshProUGUI text;
        public int id;

        public void ReleaseItem()
        {
            InventoryUI.Instance.choiceSlot = this;
            var inventory = InventoryUI.Instance;

            if (GameManager.player.inventory.items[inventory.choiceSlot.id].item == null)
                return;
            
            GameManager.player.inventory.Add(GameManager.player.inventory.items[inventory.choiceSlot.id].item, 0);
            InventoryUI.Instance.UpdateSlots();
            GameManager.player.inventory.RemoveArtifactData(GameManager.player.inventory.items[inventory.choiceSlot.id].item);
            InventoryUI.Instance.SetArtifactSlots();
        }
        public void SetItem(InventorySlot item)
        {
            if (item.item is null)
            {
                ResetItem();
                return;
            }
            
            SetSprite(item.item.itemImage);
            text.text = item.count == 0 ? "" : item.count.ToString();
        }
        
        public void ResetItem()
        {
            SetSprite(null);
            text.text = "";
        }
        
        private void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
            image.enabled = sprite != null;
        }
        public void OnDrop(PointerEventData eventData)
        {
            GameManager.player.inventory.SwapSlot(id, eventData.pointerPress.GetComponent<InventorySlotUI>().id);
            Swap(ref slot, ref eventData.pointerPress.GetComponent<InventorySlotUI>().slot);
        }

        public void Swap(ref InventorySlot a, ref InventorySlot b)
        {
            (a, b) = (b, a);
        }
    }
}
