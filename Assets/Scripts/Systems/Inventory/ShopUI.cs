using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Time;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ShopUI : MonoBehaviour
    {
        public TextMeshProUGUI shopGoldUI;

        [SerializeField] private GameObject _buyPanel;
        [SerializeField] private GameObject _sellPanel;
        [SerializeField] private GameObject _toSell;
        [SerializeField] private GameObject _toBuy;

        [SerializeField] private ShopChat shopChat;
        public ShopChat GetShopChat() => shopChat;
        [SerializeField] private CountUI countUI;
        private CountUI GetCountUI() => countUI;
        [SerializeField] private BuyUI buyUI;
        public BuyUI GetBuyUI() => buyUI;
        [SerializeField] private SellUI sellUI;
        public SellUI GetSellUI() => sellUI;
        private List<string> _shopItemIDs;
        public List<string> GetShopItemIDs() { return _shopItemIDs; }
        public void SetShopItemIDs(List<string> shopItems) { _shopItemIDs = shopItems; }

    public void ClickToSell()
        {
            _buyPanel.SetActive(false);
            _sellPanel.SetActive(true);
            _toSell.SetActive(false);
            _toBuy.SetActive(true);
            
            sellUI.ClearSellUISlot();
            sellUI.UpdateSellUISlot();
        }
        public void ClickToBuy()
        {
            _buyPanel.SetActive(true);
            _sellPanel.SetActive(false);
            _toSell.SetActive(true);
            _toBuy.SetActive(false);
        }
        public void ActiveShop()
        {
            if (gameObject.activeSelf) return;
                
            gameObject.SetActive(true);
            Game.Instance.Time.PauseTime();
        }

        public void DeActiveShop()
        {
            if (!gameObject.activeSelf) return;
            
            gameObject.SetActive(false);
            Game.Instance.Time.ResumeTime();
        }
        
        public void BuyCheck(BuySlotUI buySlotUI)
        {
            var shopItemID = _shopItemIDs[buySlotUI.GetSlotID()];
            var item = ResourceSystem.instance.GetItem(shopItemID);
            
            countUI.gameObject.SetActive(true);
            countUI.ActiveCountUI(buySlotUI);
            shopChat.BuyPriceToChat(item.price);
        }
        
        public void SellCheck(SellSlotUI sellSlotUI)
        {
            var inventory = Game.Instance.player.inventory;
            var slot = inventory.FindSlot(SlotType.Inventory, sellSlotUI.GetSlotID());
                
            if (slot.GetSlotItemID() == "") return;

            var item = slot.GetSlotItem();
            
            if (item.type == ItemType.Use || item.type == ItemType.Mineral)
            {
                UIManager.instance.shopUI.GetCountUI().gameObject.SetActive(true);
                UIManager.instance.shopUI.GetCountUI().ActiveCountUI(sellSlotUI);
                UIManager.instance.shopUI.GetShopChat().SellPriceToChat(item.price);
            }
        }
    }
}
