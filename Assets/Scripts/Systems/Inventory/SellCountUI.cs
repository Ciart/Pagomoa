using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SellCountUI : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI itemCount;
        [SerializeField] public int count = 0;

        public static SellCountUI Instance = null;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }
        public void OnUI()
        {
            transform.gameObject.SetActive(true);
            count = 0;
            itemCount.text = count.ToString();
        }
        public void OffUI()
        {
            ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
            ShopChat.Instance.CancleChat();
            transform.gameObject.SetActive(false);
        }
   
        public void Plus()
        {
            InventoryItem item = Sell.Instance.choiceSlot.inventoryItem;
            if (count < item.count)
            {
                count++;
                ShopChat.Instance.TotalPriceToChat(count * item.item.itemPrice);
            }
            else
                return;
            itemCount.text = count.ToString();
        }
        public void Minus()
        {
            {
                if (count > 0)
                {
                    count--;
                }
                else
                    return;
                itemCount.text = count.ToString();
            }
        }
        public void ClickSlot()
        {
            for (int i = 0; i < count; i++)
            {
                if (Sell.Instance.choiceSlot.inventoryItem.count > 1)
                {
                    InventoryDB.Instance.Remove(Sell.Instance.choiceSlot.inventoryItem.item);
                    QuickSlotItemDB.instance.SetCount(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                else if (Sell.Instance.choiceSlot.inventoryItem.count == 1)
                {
                    InventoryDB.Instance.Remove(Sell.Instance.choiceSlot.inventoryItem.item);
                    QuickSlotItemDB.instance.CleanSlot(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                Sell.Instance.DeleteSlot();
                Sell.Instance.ResetSlot();
            }
            count = 0;
            itemCount.text = count.ToString();
            ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
            OffUI();
            ShopChat.Instance.ThakChat();
        }
    }
}
