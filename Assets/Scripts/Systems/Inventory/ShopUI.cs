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
        [SerializeField] private GameObject _scrollView;
        [SerializeField] private GameObject _book;
        [SerializeField] private GameObject _toSell;
        [SerializeField] private GameObject _toBuy;
        [SerializeField] public ShopHover hovering;
        public TextMeshProUGUI[] playerGold;
        
        [SerializeField] private ShopChat shopChat;
        public ShopChat GetShopChat() => shopChat;
        [SerializeField] private CountUI countUI;
        public CountUI GetCountUI() => countUI;
        [SerializeField] private BuyUI buyUI;
        public BuyUI GetBuyUI() => buyUI;
        [SerializeField] private SellUI sellUI;
        public SellUI GetSellUI() => sellUI;
        private List<Item> _items;
        public List<Item> GetShopItems() => _items;
        public void SetShopItems(List<Item> shopItems) => _items = shopItems;

        public Slot chosenSlot;
        
        public void ClickToSell()
        {
            _scrollView.SetActive(false);
            _book.SetActive(true);
            _toSell.SetActive(false);
            _toBuy.SetActive(true);
            
            sellUI.DeleteSellUISlot();
            sellUI.UpdateSellUISlot();
        }
        public void ClickToBuy()
        {
            _scrollView.SetActive(true);
            _book.SetActive(false);
            _toSell.SetActive(true);
            _toBuy.SetActive(false);
        }
        public void ActiveShop()
        {
            if (gameObject.activeSelf) return;
                
            gameObject.SetActive(true);
            TimeManager.instance.PauseTime();
        }

        public void DeActiveShop()
        {
            if (!gameObject.activeSelf) return;
            
            gameObject.SetActive(false);
            TimeManager.instance.ResumeTime();
        }
    }
}
