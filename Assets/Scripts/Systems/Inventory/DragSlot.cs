using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
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
        public void OnBeginDrag(PointerEventData eventData) // ���콺 �巡�� �������� �� ȣ��
        {
            Vector3 newPosition = new Vector3(eventData.position.x, eventData.position.y);

            if (slot != null)
            {
                item = slot.inventoryItem;
                if(slot.inventoryItem.item != null )
                    DragItem.Instance.DragSetImage(slot.inventoryItem.item.itemImage);
            }
            else
            {
                if (quickSlot.inventoryItem.item == null || quickSlot.inventoryItem == null)
                    return;
                item = quickSlot.inventoryItem;
                DragItem.Instance.DragSetImage(quickSlot.inventoryItem.item.itemImage);
            }

            DragItem.Instance.transform.position = newPosition;
        }


        public void OnDrag(PointerEventData eventData) // ���콺 �巡�� ���� ���� ȣ��
        {
            if (eventData.pointerPress.GetComponent<QuickSlot>())
                return;
            else
            {
                HoverEvent.Instance.OffHover();
                DragItem.Instance.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData) // ���콺 �巡�� ������ �� ȣ��
        {
            DragItem.Instance.SetColor(0);
        }
    }
}
