using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEvent : MonoBehaviour, IPointerEnterHandler
{
    public GameObject HoverRenderer;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 newPosition = new Vector3(eventData.position.x + 5, eventData.position.y);
        Debug.Log("µé¾î¿È");
        HoverRenderer.SetActive(true);
        HoverRenderer.transform.GetChild(0).GetChild(0).position = newPosition;
    }
}
