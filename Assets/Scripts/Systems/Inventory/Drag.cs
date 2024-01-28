using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static Drag Instance;

        [SerializeField] public Slot slot;
        [SerializeField] public InventoryItem item;


        void Start()
        {
            Instance = this;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 newPosition = new Vector3(eventData.position.x, eventData.position.y);

            if (slot != null)
            {
                item = slot.inventoryItem;
                if(slot.inventoryItem.item != null )
                    DragItem.Instance.DragSetImage(slot.inventoryItem.item.itemImage);
            }
            DragItem.Instance.transform.position = newPosition;
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerPress.GetComponent<QuickSlot>())
                return;
            else
            {
                ItemHoverObject.Instance.OffHover();
                DragItem.Instance.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragItem.Instance.SetColor(0);
        }
    }
}
