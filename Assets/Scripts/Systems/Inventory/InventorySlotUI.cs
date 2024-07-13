using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Dialogue;
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
            // Inventory.Instance.choiceSlot = this;
            // var inventory = Inventory.Instance;
            //
            // if (inventory.choiceSlot.inventoryItem == null || inventory.choiceSlot.inventoryItem.item == null)
            //     return;
            //
            // InventoryDB.Instance.Add(inventoryItem.item, 0);
            // Inventory.Instance.ResetSlot();
            // ArtifactSlotDB.Instance.Remove(inventoryItem.item);
            // ArtifactContent.Instance.DeleteSlot();
            // ArtifactContent.Instance.ResetSlot();
            
            
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
            Swap(InventoryDB.Instance.items, this.id, eventData.pointerPress.GetComponent<InventorySlotUI>().id);
            Swap(this.inventoryItem, eventData.pointerPress.GetComponent<InventorySlotUI>().inventoryItem);
            // Inventory.Instance.ResetSlot();
        }
        public void Swap(List<InventoryItem> list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }
        public void Swap(InventoryItem item1, InventoryItem item2)
        {
            (item1, item2) = (item2, item1);
        }
    }
}
