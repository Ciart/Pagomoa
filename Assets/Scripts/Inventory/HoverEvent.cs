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
    [SerializeField] public GameObject image;
    [SerializeField] private GameObject itemName;
    [SerializeField] private GameObject itemInfo;

    void Start()
    {
        Instance = this;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.inventoryItem.item == null)
        {
            OffHover();
        }
        else if (slot.inventoryItem != null)
        {
            Vector3 newPosition = new Vector3(eventData.position.x + 5, eventData.position.y);
            image.SetActive(true);
            image.transform.position = newPosition;
            itemName.GetComponent<Text>().text = slot.inventoryItem.item.itemName;
            itemInfo.GetComponent<Text>().text = slot.inventoryItem.item.itemInfo;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.SetActive(false);
    }
    public void OffHover()
    {
        if (image.activeSelf == false)
            return;
        image.SetActive(false);
    }
}
