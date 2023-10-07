using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    static public HoverEvent Instance;

    [SerializeField] private Slot _slot;
    [SerializeField] public GameObject HoverRenderer;
    [SerializeField] private GameObject _itemName;
    [SerializeField] private GameObject _itemInfo;

    void Start()
    {
        Instance = this;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        var slot = eventData.pointerEnter.GetComponent<Slot>();
        if (slot.inventoryItem == null || slot.inventoryItem.item == null)
            OffHover();
        
        else if (_slot.inventoryItem != null)
        {
            Vector3 newPosition = new Vector3(eventData.position.x + 5, eventData.position.y);
            HoverRenderer.SetActive(true);
            HoverRenderer.transform.position = newPosition;
            _itemName.GetComponent<Text>().text = _slot.inventoryItem.item.itemName;
            _itemInfo.GetComponent<Text>().text = _slot.inventoryItem.item.itemInfo;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverRenderer.SetActive(false);
    }
    public void OffHover()
    {
        if (HoverRenderer.activeSelf == false)
            return;
        HoverRenderer.SetActive(false);
    }
}
