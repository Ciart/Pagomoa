using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SellSlot : MonoBehaviour, ISlot
    {
        public Slot slot { get; private set; }
        
        [Header("판매 슬롯 변수")]
        public Image itemImage;
        public TextMeshProUGUI countText;
        public int id;
        
        private void Awake()
        {
            slot = new Slot();
            slot.SetSlotType(SlotType.Sell);
        }

        public SlotType GetSlotType() { return slot.GetSlotType(); }
        public int GetSlotID() { return id; }

        public void SetSlot(Slot targetSlot)
        {
            if (targetSlot.GetSlotItemID() == "")
            {
                ResetSlot();
            }
            else if (targetSlot.GetSlotItem().id != "")
            {
                slot.SetSlotItemID(targetSlot.GetSlotItem().id);
                slot.SetSlotItemCount(targetSlot.GetSlotItemCount());
                itemImage.sprite = targetSlot.GetSlotItem().sprite;
                countText.text = slot.GetSlotItemCount() == 0 ? "" : slot.GetSlotItemCount().ToString();
            }
        }

        public void ResetSlot()
        {
            slot.SetSlotItemID("");
            slot.SetSlotItemCount(0);   
            itemImage.sprite = null;
            countText.text = "";
        }

        /*public void ClickSlot()
        {
            var inventory = Game.instance.player.inventory;
            var shopUI = UIManager.instance.shopUI;
            var countUI = shopUI.GetCountUI();
            
            for (int i = 0; i < countUI.inputCount; i++)
            {
                if (slot.GetSlotItemCount() > 1)
                {
                    inventory.SellItem(slot.GetSlotItem());
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                else if (slot.GetSlotItemCount() == 1)
                {
                    inventory.SellItem(slot.GetSlotItem());
                    // QuickSlotUI.instance.SetQuickSlotItemCount(Buy.Instance.choosenSellSlot.inventoryItem.item);
                }
                shopUI.GetSellUI().DeleteSellUISlot();
                shopUI.GetSellUI().UpdateSellUISlot();
            }
            
            countUI.inputCount = 0;
            countUI.countUIText.text = countUI.inputCount.ToString();
            countUI.DisableCountUI();
        }*/
    }
}
