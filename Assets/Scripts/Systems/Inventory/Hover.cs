using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public Sprite[] hoverImage;
        [SerializeField] public Image boostImage;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            boostImage.sprite = hoverImage[0];
            Inventory.Instance.hoverSlot = this.gameObject.GetComponent<Slot>();
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            boostImage.sprite = hoverImage[1];
            Inventory.Instance.hoverSlot = null;
        }
    }
}
