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
    public class QuickSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] public Image itemImage;
        [SerializeField] public Sprite transparentImage;
        [SerializeField] public TextMeshProUGUI itemCount;

        [SerializeField] public int id;
        [SerializeField] public InventoryItem inventoryItem;
        [SerializeField] public Image selectedSlotImage;
        
        private void AddSlot(InventoryItem data)
        {
            int idx = Array.FindIndex(GameManager.player.inventoryDB.quickSlots, element => element.item == data.item);
            
            if (idx == -1)
            {
                GameManager.player.inventoryDB.quickSlots[id] = data;
            }
            else
                return;

            SetImage();
        }
        private void ChangeSlot(PointerEventData eventData)
        {
            Swap(GameManager.player.inventoryDB.quickSlots, this.id, eventData.pointerDrag.GetComponent<QuickSlot>().id);
        }
        private void SetSlotNull()
        {
            inventoryItem = null;
            itemImage.sprite = transparentImage;
            itemCount.text = null;
        }
        private void SetImage()
        {
            itemImage.sprite = inventoryItem.item.itemImage;
            if (inventoryItem.count != 0)
                SetItemCount();
        }
        private void SetItemCount()
        {
            itemCount.text = inventoryItem.count.ToString();
        }
        
        private void Swap(InventoryItem[] quickSlot, int i, int j)
        {
            (quickSlot[i], quickSlot[j]) = (quickSlot[j], quickSlot[i]);
            
            for (int r = 0; r < GameManager.player.inventoryDB.quickSlots.Length; r++)
            {
                QuickSlotUI.instance.quickSlotsUI[r].SetSlotNull();
                QuickSlotUI.instance.quickSlotsUI[r].inventoryItem = GameManager.player.inventoryDB.quickSlots[r];
                
                if (QuickSlotUI.instance.quickSlotsUI[r].inventoryItem.item != null)
                    QuickSlotUI.instance.quickSlotsUI[r].SetImage();
                
                else if (QuickSlotUI.instance.quickSlotsUI[r].inventoryItem.item == null)
                    QuickSlotUI.instance.quickSlotsUI[r].itemImage.sprite = transparentImage;
            }
        }
        public void OnDrop(PointerEventData eventData)
        {
            Drag dragSlot = eventData.pointerDrag.GetComponent<Drag>();
            this.inventoryItem = dragSlot.item;

            if (eventData.pointerDrag.GetComponent<QuickSlot>())
                ChangeSlot(eventData);
            else if (eventData.pointerDrag.GetComponent<InventorySlotUI>())
                AddSlot(dragSlot.item);
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


        public void UseItem()
        {
            PlayerStatus playerPlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            QuickSlotUI.instance.selectedSlot.inventoryItem.item.Active(playerPlayerStatus);
        }
    }
}
