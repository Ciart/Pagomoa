using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        [FormerlySerializedAs("inventoryItem")] public InventorySlot slot;
        public Image image;
        public TextMeshProUGUI text;
        public int id;

        public void ReleaseItem()
        {
            InventoryUI.Instance.choiceSlot = this;
            var inventory = InventoryUI.Instance;

            if (inventory.choiceSlot.slot.item == null)
                return;
            
            Debug.Log(this.slot.item);
            GameManager.player.inventory.Add(this.slot.item, 0);
            InventoryUI.Instance.UpdateSlots();
            GameManager.player.inventory.RemoveArtifactData(this.slot.item);
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
            Swap(GameManager.player.inventory.items, this.id, eventData.pointerPress.GetComponent<InventorySlotUI>().id);
            Swap(this.slot, eventData.pointerPress.GetComponent<InventorySlotUI>().slot);
            InventoryUI.Instance.UpdateSlots();
        }
        public void Swap(InventorySlot[] list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }
        public void Swap(InventorySlot item1, InventorySlot item2)
        {
            (item1, item2) = (item2, item1);
        }
    }
}
