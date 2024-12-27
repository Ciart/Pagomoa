using Ciart.Pagomoa.Systems.Dialogue;
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
        
        public void ClickToSell()
        {
            _scrollView.SetActive(false);
            _book.SetActive(true);
            _toSell.SetActive(false);
            _toBuy.SetActive(true);
            
            var buy = UIManager.instance.shopUI.GetBuyUI();
            buy.DeleteSellUISlot();
            buy.ResetSellUISlot();
        }
        public void ClickToBuy()
        {
            _scrollView.SetActive(true);
            _book.SetActive(false);
            _toSell.SetActive(true);
            _toBuy.SetActive(false);
        }
        public void SetUI()
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
        }
    }
}
