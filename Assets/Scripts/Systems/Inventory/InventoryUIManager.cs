using UnityEngine;
using UnityEngine.InputSystem.Switch;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _itemsTab;
    [SerializeField] private Button _itemsTabButton;
    [SerializeField] private GameObject _infoTab;
    [SerializeField] private Button _infoTabButton;
    [SerializeField] private Sprite[] _tabSprites;

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
        _itemsTabButton.image.sprite = _tabSprites[0];
        _infoTabButton.image.sprite = _tabSprites[2];
    }
    public void ClickInfoTab()
    {
        _infoTab.SetActive(true);
        _itemsTab.SetActive(false);
        _infoTabButton.image.sprite = _tabSprites[3];
        _itemsTabButton.image.sprite = _tabSprites[1];
    }
}