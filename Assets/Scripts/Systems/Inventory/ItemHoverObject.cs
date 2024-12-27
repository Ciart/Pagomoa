using Ciart.Pagomoa.Systems.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa
{
    public class ItemHoverObject : MonoBehaviour
    {
        public static ItemHoverObject Instance = null;

        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemType;
        [SerializeField] private TextMeshProUGUI _itemInfo;

        private void Start()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }
        public void WriteText(InventorySlotUI slot)
        {
            _itemName.GetComponent<TextMeshProUGUI>().text = slot.slot.item.name;
            _itemInfo.GetComponent<TextMeshProUGUI>().text = slot.slot.item.description;
            _itemType.GetComponent<TextMeshProUGUI>().text = slot.slot.item.type.ToString();
        }
        public void OffHover()
        {
            if (this.gameObject.activeSelf == false)
                return;
            this.gameObject.SetActive(false);
        }
    }
}
