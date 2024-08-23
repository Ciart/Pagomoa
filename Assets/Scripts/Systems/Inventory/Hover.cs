using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public Sprite[] hoverImage;
        [SerializeField] public Image boostImage;
        
        private void OnEnable()
        {
            boostImage.sprite = hoverImage[1];
            InventoryUI.Instance.hoverSlot = null;
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            boostImage.sprite = hoverImage[0];
            InventoryUI.Instance.hoverSlot = this.gameObject.GetComponent<InventorySlotUI>();
        }
        
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            boostImage.sprite = hoverImage[1];
            InventoryUI.Instance.hoverSlot = null;
        }
    }
}
