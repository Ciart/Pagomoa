using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        public static QuickSlot Instance;

        [SerializeField] public Image itemImage;
        [SerializeField] public Sprite transparentImage;
        [SerializeField] public TextMeshProUGUI itemCount;

        [SerializeField] public int id;
        [SerializeField] public EtcInventory inventory;
        [SerializeField] public InventoryItem inventoryItem;
        [SerializeField] public Image selectedSlotImage;
        [SerializeField] private QuickSlotItemDB quickSlotItemDB;
        void Start()
        {
            Instance = this;
        }
        public void OnDrop(PointerEventData eventData)
        {
            DragSlot dragSlot = eventData.pointerDrag.GetComponent<DragSlot>();
            dragSlot.item = eventData.pointerDrag.GetComponent<DragSlot>().item;

            this.inventoryItem = dragSlot.item;
            if (eventData.pointerDrag.GetComponent<QuickSlot>())
            {
                ChangeSlot(eventData);
            }
            else if (eventData.pointerDrag.GetComponent<Slot>())
            {
                AddSlot(dragSlot.item);
            }
        }
        private void AddSlot(InventoryItem data)
        {
            //var quickslotitem = QuickSlotItemDB.instance.quickSlotItems.Find(quickslotitem => quickslotitem.item == data);

            if (!QuickSlotItemDB.instance.quickSlotItems.Contains(data)/*quickslotitem == null*/)
            {
                QuickSlotItemDB.instance.quickSlotItems.Insert(this.id, inventoryItem);
                QuickSlotItemDB.instance.quickSlotItems.RemoveAt(this.id + 1);
            }
            else
                return;

            SetImage();
        }
        private void ChangeSlot(PointerEventData eventData)
        {
            Swap(QuickSlotItemDB.instance.quickSlotItems, this.id, eventData.pointerDrag.GetComponent<QuickSlot>().id);
            SetQuickSlot();
        }
        public void SetSlotNull()
        {
            inventoryItem = null;
            itemImage.sprite = transparentImage;
            itemCount.text = null;
        }
        public void SetItemCount()
        {
            itemCount.text = inventoryItem.count.ToString();
        }
        public void SetQuickSlot()
        {
            for (int i = 0; i < QuickSlotItemDB.instance.quickSlotItems.Count; i++)
            {
                QuickSlotItemDB.instance.quickSlots[i].SetSlotNull();
                QuickSlotItemDB.instance.quickSlots[i].inventoryItem = QuickSlotItemDB.instance.quickSlotItems[i];
            }
            for (int i = 0; i < QuickSlotItemDB.instance.quickSlotItems.Count; i++)
            {
                if (QuickSlotItemDB.instance.quickSlots[i].inventoryItem.item != null)
                    QuickSlotItemDB.instance.quickSlots[i].SetImage();

                else if (QuickSlotItemDB.instance.quickSlots[i].inventoryItem.item == null)
                    QuickSlotItemDB.instance.quickSlots[i].itemImage.sprite = transparentImage;
            }
        }
        public void SetImage()
        {
            itemImage.sprite = inventoryItem.item.itemImage;
            if (inventoryItem.count != 0)
                SetItemCount();
            else
                return;
        }
        public void Swap(List<InventoryItem> list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }
        public void Swap(InventoryItem item1, InventoryItem item2)
        {
            (item1, item2) = (item2, item1);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (inventoryItem == null || inventoryItem.item == null)
                return;
            Vector3 newPosition = new Vector3(eventData.position.x, eventData.position.y);
            DragItem.Instance.DragSetImage(inventoryItem.item.itemImage);
            DragItem.Instance.transform.position = newPosition;
        }
        public void OnDrag(PointerEventData eventData)
        {
            DragItem.Instance.transform.position = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.Instance.SetColor(0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));
            for (int i = 0; i < QuickSlotItemDB.instance.quickSlots.Count; i++)
            {
                QuickSlotItemDB.instance.quickSlots[i].selectedSlotImage.gameObject.SetActive(false);
            }

            QuickSlotItemDB.instance.selectedSlot = eventData.pointerPress.GetComponent<QuickSlot>();
            QuickSlotItemDB.instance.selectedSlot.selectedSlotImage.gameObject.SetActive(true);
            QuickSlotItemDB.instance.selectedSlot.transform.SetAsLastSibling();
        }



        ////
        ///


        public void UseItem()
        {
            PlayerStatus playerPlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            QuickSlotItemDB.instance.selectedSlot.inventoryItem.item.Active(playerPlayerStatus);
        }
    }
}
