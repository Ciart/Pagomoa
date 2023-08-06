using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Canvas canvas;
    
    


    public InventoryItem inventoryItem;
    [SerializeField] private DragItem dragItem;
    [SerializeField] private EquipUI equipUI;
    [SerializeField] private SellCountUI sellCountUI;
    [SerializeField] private BuyCountUI buyCountUI;
    [SerializeField] private BuyNoCountUI buyNoCountUI;
    [SerializeField] public Image image;
    [SerializeField] public Text text;

    public void SellCheck()
    {
        EtcInventory.Instance.choiceSlot = this;
        if (EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use ||
            EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Mineral)
        {
            sellCountUI.OnUI();
            sellCountUI.ItemImage(EtcInventory.Instance.choiceSlot.inventoryItem.item.itemImage);
        }
    }
    public void ClickSlot()
    {
        for (int i = 0; i < sellCountUI.count; i++)
        {
            EtcInventory.Instance.DeleteSlot();
            InventoryDB.Instance.Remove(EtcInventory.Instance.choiceSlot.inventoryItem.item);
            EtcInventory.Instance.UpdateSlot();
        }
        sellCountUI.count = 0;
        sellCountUI.price = 0;
        sellCountUI.itemCount.text = sellCountUI.count.ToString();
        sellCountUI.totalPrice.text = sellCountUI.price.ToString();
        sellCountUI.OffUI();
    }
    public void BuyCheck()
    {
        Buy.Instance.choiceSlot = this;
        if (Buy.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            buyCountUI.OnUI();
            buyCountUI.ItemImage(Buy.Instance.choiceSlot.inventoryItem.item.itemImage);
        }
        else if(Buy.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
        {
            buyNoCountUI.OnUI();
            buyNoCountUI.ItemImage(Buy.Instance.choiceSlot.inventoryItem.item.itemImage);
        }
    }
    public void BuySlot()
    {
        if (Buy.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            if (InventoryDB.Instance.Gold >= Buy.Instance.choiceSlot.inventoryItem.item.itemPrice)
            {
                Buy.Instance.choiceSlot.inventoryItem.count -= buyCountUI.count;
                InventoryDB.Instance.Add(Buy.Instance.choiceSlot.inventoryItem.item, buyCountUI.count);
                AuctionDB.Instance.Remove(Buy.Instance.choiceSlot.inventoryItem.item);
                Buy.Instance.UpdateSlot();
                buyCountUI.OffUI();
            }
            else
                return;
            
            if (Buy.Instance.choiceSlot.inventoryItem.count == 0)
            {
                AuctionDB.Instance.Remove(Buy.Instance.choiceSlot.inventoryItem.item);
                Buy.Instance.DestroySlot();
                Buy.Instance.AuctionSlot();
                Buy.Instance.UpdateSlot();
                buyCountUI.OffUI();
            }
        }

        else if (Buy.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
        {
            if (InventoryDB.Instance.Gold >= Buy.Instance.choiceSlot.inventoryItem.item.itemPrice)
            {
                InventoryDB.Instance.Add(Buy.Instance.choiceSlot.inventoryItem.item, Buy.Instance.choiceSlot.inventoryItem.count);
                AuctionDB.Instance.Remove(Buy.Instance.choiceSlot.inventoryItem.item);
                Buy.Instance.DestroySlot();
                Buy.Instance.AuctionSlot();
                Buy.Instance.UpdateSlot();
                buyNoCountUI.OffUI();
            }
            else
                return;
        }
    }
    public void EquipCheck()
    {
        EtcInventory.Instance.choiceSlot = this;
        if(EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Equipment)
            equipUI.OnUI();

        else if(EtcInventory.Instance.choiceSlot.inventoryItem.item.itemType == Item.ItemType.Use)
        {
            EtcInventory.Instance.choiceSlot.inventoryItem.count -= 1;
            if (EtcInventory.Instance.choiceSlot.inventoryItem.count == 0)
            {
                InventoryDB.Instance.items.Remove(EtcInventory.Instance.choiceSlot.inventoryItem);
                EtcInventory.Instance.DeleteSlot();
            }
            EtcInventory.Instance.UpdateSlot();
        }
    }
    public void EquipItem()
    {
        if (ArtifactSlotDB.Instance.Artifact.Count < 4 && inventoryItem != null)
        {
            EtcInventory.Instance.DeleteSlot();
            ArtifactSlotDB.Instance.Artifact.Add(EtcInventory.Instance.choiceSlot.inventoryItem);
            InventoryDB.Instance.Equip(EtcInventory.Instance.choiceSlot.inventoryItem.item);
            EtcInventory.Instance.UpdateSlot();
            ArtifactContent.Instance.ResetSlot();
        }
        else
            return;
        equipUI.OffUI();
    }
    public void ReleaseItem()
    {
        InventoryDB.Instance.items.Add(inventoryItem);
        EtcInventory.Instance.ResetSlot();
        ArtifactSlotDB.Instance.Remove(inventoryItem.item);
        ArtifactContent.Instance.ResetSlot();
        EtcInventory.Instance.DeleteSlot();
        EtcInventory.Instance.UpdateSlot();
        ArtifactContent.Instance.DeleteSlot();
        ArtifactContent.Instance.UpdateSlot();
    }
    public void SetUI(Sprite s, string m)
    {
        image.sprite = s;
        text.text = m;
    }
    public void SetUI(Sprite s)
    {
        image.sprite = s;
    }
    public void Return()
    {
        equipUI.OffUI();
        return;
    }
    public void OnBeginDrag(PointerEventData eventData) // 마우스 드래그 시작했을 때 호출
    {
        canvas = transform.root.GetComponentInChildren<Canvas>();
        RectTransform rt = canvas.transform as RectTransform;

        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, mousePosition, canvas.worldCamera, out localPosition);
       


        EtcInventory.Instance.choiceSlot = this;
        dragItem.gameObject.SetActive(true);
        dragItem.DragSetImage(EtcInventory.Instance.choiceSlot.inventoryItem.item.itemImage);

        RectTransform rtImage = dragItem.transform as RectTransform;
        rt.anchoredPosition = localPosition;

        dragItem.transform.position = rt.anchoredPosition;
        Debug.Log(eventData.position);
    }

    public void OnDrag(PointerEventData eventData) // 마우스 드래그 중인 동안 호출
    {
        Debug.Log("드래그중");
    }

    public void OnEndDrag(PointerEventData eventData) // 마우스 드래그 끝났을 때 호출
    {
        Debug.Log("끝났음");
        DragItem.Instance.gameObject.SetActive(false);
    }
}