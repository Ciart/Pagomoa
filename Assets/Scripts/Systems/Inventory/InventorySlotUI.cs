using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        public InventoryItem inventoryItem;
        public Image image;
        public TextMeshProUGUI text;
        public int id;

        public void ReleaseItem()
        {
            InventoryUI.Instance.choiceSlot = this;
            var inventory = InventoryUI.Instance;

            if (inventory.choiceSlot.inventoryItem.item == null)
                return;
            
            Debug.Log(this.inventoryItem.item);
            GameManager.player.inventoryDB.Add(this.inventoryItem.item, 0);
            InventoryUI.Instance.ResetSlot();
            GameManager.player.inventoryDB.RemoveArtifactData(this.inventoryItem.item);
            InventoryUI.Instance.SetArtifactSlots();
        }
        public void SetItem(InventoryItem item)
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
            Swap(GameManager.player.inventoryDB.items, this.id, eventData.pointerPress.GetComponent<InventorySlotUI>().id);
            Swap(this.inventoryItem, eventData.pointerPress.GetComponent<InventorySlotUI>().inventoryItem);
            InventoryUI.Instance.ResetSlot();
        }
        public void Swap(InventoryItem[] list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }
        public void Swap(InventoryItem item1, InventoryItem item2)
        {
            (item1, item2) = (item2, item1);
        }
    }
}
