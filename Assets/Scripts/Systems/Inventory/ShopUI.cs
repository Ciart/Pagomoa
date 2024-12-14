using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject _scrollView;
        [SerializeField] private GameObject _book;
        [SerializeField] private GameObject _toSell;
        [SerializeField] private GameObject _toBuy;
        [SerializeField] public ShopHover hovering;
        [SerializeField] public TextMeshProUGUI[] gold;

        public static ShopUI Instance = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        public void ClickToSell()
        {
            _scrollView.SetActive(false);
            _book.SetActive(true);
            _toSell.SetActive(false);
            _toBuy.SetActive(true);
            var buy = GetComponent<Buy>();
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
            bool click = false;
            if (gameObject.activeSelf == false)
                click = !click;
            gameObject.SetActive(click);
        }
    }
}
