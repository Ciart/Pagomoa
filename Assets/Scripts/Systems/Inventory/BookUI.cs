using Ciart.Pagomoa.UI;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BookUI : MonoBehaviour
    {
        [SerializeField] private GameObject _itemsTab;
        [SerializeField] private TabSpriteButton _itemsTabButton;
        [SerializeField] private GameObject _infoTab;
        [SerializeField] private TabSpriteButton _infoTabButton;
        [SerializeField] private GameObject _questTab;
        [SerializeField] private TabSpriteButton _questTabButton;
        [SerializeField] private Sprite[] _tabSprites;
        [SerializeField] public GameObject ItemHoverObject;
        
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
            _itemsTabButton.isSelected = true;
            _infoTabButton.isSelected = false;
            _questTabButton.isSelected = false;
        }
        public void ClickInfoTab()
        {
            _infoTab.SetActive(true);
            _itemsTab.SetActive(false);
            _questTab.SetActive(false);
            _infoTabButton.isSelected = true;
            _itemsTabButton.isSelected = false;
            _questTabButton.isSelected = false;
        }
        public void ClickQuestTab()
        {
            _questTab.SetActive(true);
            _itemsTab.SetActive(false);
            _infoTab.SetActive(false);
            _questTabButton.isSelected = true;
            _itemsTabButton.isSelected = false;
            _infoTabButton.isSelected = false;
        }
    }
}
