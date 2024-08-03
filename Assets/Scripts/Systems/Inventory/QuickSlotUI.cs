using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class QuickSlotUI : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] public Image itemImage;
        [SerializeField] public Sprite transparentImage;
        [SerializeField] public TextMeshProUGUI itemCount;

        [SerializeField] public int id;
        [SerializeField] public Image selectedSlotImage;
        
        private Item _item
        {
            get => GameManager.player.inventory.GetQuickItem(id);
            set => GameManager.player.inventory.SetQuickItem(id, value);
        } 

        private void SwapSlot(PointerEventData eventData)
        {
            GameManager.player.inventory.SwapQuickSlot(id,  eventData.pointerDrag.GetComponent<QuickSlotUI>().id);
        }
        
        public void UpdateSlot()
        {
            itemImage.sprite = _item?.itemImage ?? transparentImage;
            itemCount.text =  GameManager.player.inventory.GetItemCount(_item).ToString();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var dragSlot = eventData.pointerDrag.GetComponent<Drag>();

            if (eventData.pointerDrag.GetComponent<QuickSlotUI>())
                SwapSlot(eventData);
            else if (eventData.pointerDrag.GetComponent<InventorySlotUI>())
                _item = dragSlot.item.item;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_item == null)
                return;
            
            var newPosition = new Vector3(eventData.position.x, eventData.position.y);
            DragItem.Instance.DragSetImage(_item.itemImage);
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



        ////
        ///
        //
        //
        // public void UseItem()
        // {
        //     PlayerStatus playerPlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        //     QuickSlotContainerUI.instance.selectedSlot.inventoryItem.item.Active(playerPlayerStatus);
        // }
    }
}
