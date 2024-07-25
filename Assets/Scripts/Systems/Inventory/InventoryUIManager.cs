using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventoryUIManager : MonoBehaviour
    {
        public static InventoryUIManager Instance = null;

        [SerializeField] private GameObject _itemsTab;
        [SerializeField] private Button _itemsTabButton;
        [SerializeField] private GameObject _infoTab;
        [SerializeField] private Button _infoTabButton;
        [SerializeField] private GameObject _questTab;
        [SerializeField] private Button _questTabButton;
        [SerializeField] private Sprite[] _tabSprites;
        [SerializeField] public GameObject ItemHoverObject;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        public void SetUI()
        {
            bool click = false;
            if (gameObject.activeSelf == false)
                click = !click;
            gameObject.SetActive(click);
        }
        public void ClickItemsTab()
        {
            _itemsTab.SetActive(true);
            _infoTab.SetActive(false);
            _questTab.SetActive(false);
            _itemsTabButton.image.sprite = _tabSprites[0];
            _infoTabButton.image.sprite = _tabSprites[2];
        }
        public void ClickInfoTab()
        {
            _infoTab.SetActive(true);
            _itemsTab.SetActive(false);
            _questTab.SetActive(false);
            _infoTabButton.image.sprite = _tabSprites[3];
            _itemsTabButton.image.sprite = _tabSprites[1];
        }
        public void ClickQuestTab()
        {
            _questTab.SetActive(true);
            _itemsTab.SetActive(false);
            _infoTab.SetActive(false);
            //_infoTabButton.image.sprite = _tabSprites[3];
            //_itemsTabButton.image.sprite = _tabSprites[1];
        }
    }
}
