using UnityEngine;
using UnityEngine.InputSystem.Switch;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _itemsTab;
    [SerializeField] private GameObject _infoTab;
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
        Transform[] infoChildren = _infoTab.GetComponentsInChildren<Transform>();
        foreach (Transform child in infoChildren)
        {
            if (_infoTab.transform == child)
                child.gameObject.SetActive(true);
            else
            {
                child.gameObject.SetActive(false);
            }
        }

        Transform[] itemsChildren = _itemsTab.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in itemsChildren)
        {
            child.gameObject.SetActive(true);
        }
        _itemsTab.GetComponent<Image>().sprite = _tabSprites[0];
        _infoTab.GetComponent<Image>().sprite = _tabSprites[2];
    }
    public void ClickInfoTab()
    {
        Transform[] itemsChildren = _itemsTab.GetComponentsInChildren<Transform>();
        foreach (Transform child in itemsChildren)
        {
            if (_itemsTab.transform == child)
                child.gameObject.SetActive(true);
            else
            {
                Debug.Log(child.name);
                child.gameObject.SetActive(false);
            }
        }
        Transform[] infoChildren = _infoTab.GetComponentsInChildren<Transform>(true);
        foreach(Transform child in infoChildren)
        {
            child.gameObject.SetActive(true);
        }

        _itemsTab.GetComponent<Image>().sprite = _tabSprites[1];
        _infoTab.GetComponent<Image>().sprite = _tabSprites[3];
    }
}
