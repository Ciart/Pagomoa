using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlot : Slot
    {
        public Image slotImage;
        [SerializeField] public Image itemImage;
        
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private TextMeshProUGUI countText;
        
        public int id;
        [HideInInspector] public int dependentID = -1; 

        private void Awake()
        {
            SetSlotType(SlotType.Quick);
            slotImage = GetComponent<Image>();
            countText.color = new Color(1, 1, 1, 255);
        }
        
        public override void SetSlot(Slot slot)
        {
            if (slot.GetSlotItem().id != "")
            {
                SetSlotItem(slot.GetSlotItem());
                SetSlotItemCount(slot.GetSlotItemCount());
                itemImage.sprite = slot.GetSlotItem().sprite;
                countText.text = GetSlotItemCount() == 0 ? "" : GetSlotItemCount().ToString();
            }
            else
            {
                itemImage.sprite = _emptySprite;
                countText.text = "";
                GetSlotItem().ClearItemProperty();
                SetSlotItemCount(0);
            } 
        }
        
        public void ResetSlot()
        {
            Debug.Log("in " + dependentID);
            var emptyItem = new Item();
            SetSlotItem(emptyItem);
            SetSlotItemCount(0);
            
            itemImage.sprite = _emptySprite;
            countText.text = "";
        }

        /*public void OnDrop(PointerEventData eventData)
        {
            var dragSlot = eventData.pointerDrag.GetComponent<Drag>();

            if (eventData.pointerDrag.GetComponent<QuickSlot>())
                SwapSlot(eventData);
            else if (eventData.pointerDrag.GetComponent<InventorySlot>())
            {
                //_item = dragSlot.item.GetSlotItem();
            }
                
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_item == null)
                return;
            
            var newPosition = new Vector3(eventData.position.x, eventData.position.y);
            DragItem.instance.DragSetImage(_item.sprite);
            DragItem.instance.transform.position = newPosition;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            DragItem.instance.transform.position = eventData.position;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.instance.SetColor(0);
        }
        */

        // public void OnPointerClick(PointerEventData eventData)
        // {
        //     Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
        //         Input.mousePosition.y, -Camera.main.transform.position.z));
        //     for (int i = 0; i < QuickSlotUI.instance.quickSlotsUI.Length; i++)
        //     {
        //         QuickSlotUI.instance.quickSlotsUI[i].selectedSlotImage.gameObject.SetActive(false);
        //     }
        //
        //     QuickSlotUI.instance.selectedSlot = eventData.pointerPress.GetComponent<QuickSlot>();
        //     QuickSlotUI.instance.selectedSlot.selectedSlotImage.gameObject.SetActive(true);
        //     QuickSlotUI.instance.selectedSlot.transform.SetAsLastSibling();
        // }
        
        // public void UseItem()
        // {
        //     PlayerStatus playerPlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        //     QuickSlotContainerUI.instance.selectedSlot.inventoryItem.item.Active(playerPlayerStatus);
        // }
    }
}
