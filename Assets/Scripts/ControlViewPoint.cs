using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlViewPoint : MonoBehaviour, IPointerEnterHandler
{
    public MouseEvent mouseEvent;
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEvent.HoverRenderer.SetActive(false);
    }
}
