using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Sprite[] hoverImage;
    [SerializeField] public Image boostImage;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        boostImage.sprite = hoverImage[0];
        EtcInventory.Instance.hoverSlot = this.gameObject.GetComponent<Slot>();
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        boostImage.sprite = hoverImage[1];
        EtcInventory.Instance.hoverSlot = null;
    }
}
