using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    static public HoverEvent Instance;

    [SerializeField] private Slot slot;
    [SerializeField] public GameObject hoverRenderer;
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject itemName;
    [SerializeField] private GameObject itemInfo;

    void Start()
    {
        Instance = this;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.inventoryItem != null)
        {
            Vector3 newPosition = new Vector3(eventData.position.x + 5, eventData.position.y);
            hoverRenderer.SetActive(true);
            //image.transform.position = slot.transform.position;
            image.transform.position = newPosition;
            itemName.GetComponent<Text>().text = slot.inventoryItem.item.itemName;
            itemInfo.GetComponent<Text>().text = slot.inventoryItem.item.itemInfo;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverRenderer.SetActive(false);
    }
}