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
            Inventory.Instance.choiceSlot = this;
            var inventory = Inventory.Instance;

            if (inventory.choiceSlot.inventoryItem == null || inventory.choiceSlot.inventoryItem.item == null)
                return;
            
            Debug.Log(this.inventoryItem.item);
            GameManager.player.inventoryDB.Add(this.inventoryItem.item, 0);
            Inventory.Instance.ResetSlot();
            GameManager.player.inventoryDB.RemoveArtifactData(this.inventoryItem.item);
            Inventory.Instance.SetArtifactSlots();
        }
        public void SetUI(Sprite s, string m)
        {
            image.sprite = s;
            text.text = m;
        }
        public void SetUI(Sprite s)
        {
            image.sprite = s;
        }
        public void OnDrop(PointerEventData eventData)
        {
            Swap(GameManager.player.inventoryDB.items, this.id, eventData.pointerPress.GetComponent<Slot>().id);
            Swap(this.inventoryItem, eventData.pointerPress.GetComponent<Slot>().inventoryItem);
            Inventory.Instance.ResetSlot();
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
