using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static QuickSlot Instance;

    [SerializeField] private InventoryItem inventoryItem;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCount;

    void Start()
    {
        Instance = this;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerPress);
        inventoryItem = eventData.pointerPress.GetComponent<DragSlot>().item;
        ChangeSlot();
    }
    private void ChangeSlot()
    {
        QuickSlotItemDB.instance.QuickSlotItems.Add(inventoryItem);
        itemImage.sprite = inventoryItem.item.itemImage;
        if (inventoryItem.count != 0)
            itemCount.text = inventoryItem.count.ToString();
        else
            return;
    }
    private void SetSlotNull()
    {
        inventoryItem = null;
        itemImage.sprite = null;
        itemCount.text = null;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
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
        SetSlotNull();
    }
}
