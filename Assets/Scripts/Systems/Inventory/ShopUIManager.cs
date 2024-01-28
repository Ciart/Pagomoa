using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ShopUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _scrollView;
        [SerializeField] private GameObject _book;
        [SerializeField] private GameObject _toSell;
        [SerializeField] private GameObject _toBuy;
        [SerializeField] public ShopHover hovering;
        [SerializeField] public TextMeshProUGUI[] gold;

        public static ShopUIManager Instance = null;

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
