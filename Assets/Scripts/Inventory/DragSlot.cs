using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static DragSlot Instance;

    [SerializeField] public Slot slot;
    [SerializeField] private QuickSlot quickSlot;
    [SerializeField] public GameObject image;
    [SerializeField] public InventoryItem item;


    void Start()
    {
        Instance = this;
    }
    public void OnBeginDrag(PointerEventData eventData) // 마우스 드래그 시작했을 때 호출
    {
        Vector3 newPosition = new Vector3(eventData.position.x, eventData.position.y);

        if (slot != null)
        {
            item = slot.inventoryItem;
            DragItem.Instance.DragSetImage(slot.inventoryItem.item.itemImage);
        }
        else
        {
            item = quickSlot.inventoryItem;
            DragItem.Instance.DragSetImage(quickSlot.inventoryItem.item.itemImage);
        }

        DragItem.Instance.transform.position = newPosition;
    }


    public void OnDrag(PointerEventData eventData) // 마우스 드래그 중인 동안 호출
    {
        HoverEvent.Instance.image.SetActive(false);
        DragItem.Instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // 마우스 드래그 끝났을 때 호출
    {
        DragItem.Instance.SetColor(0);
    }
}
