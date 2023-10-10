using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellCountUI : MonoBehaviour
{
    [SerializeField] private Image _item;
    [SerializeField] public TextMeshProUGUI itemCount;
    [SerializeField] public TextMeshProUGUI totalPrice;
    [SerializeField] public int price = 0;
    [SerializeField] public int count = 0;

    public static SellCountUI Instance = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    public void OnUI()
    {
        transform.gameObject.SetActive(true);
        price = 0;
        count = 0;
        totalPrice.text = price.ToString();
        itemCount.text = count.ToString();
    }
    public void OffUI()
    {
        transform.gameObject.SetActive(false);
    }
    public void ItemImage(Sprite image)
    {
        _item.sprite = image;
    }
    public void Plus()
    {
        InventoryItem item = Sell.Instance.choiceSlot.inventoryItem;
        if (count < item.count)
        {
            count++;
            price += item.item.itemPrice;
        }
        else
            return;
        totalPrice.text = price.ToString();
        itemCount.text = count.ToString();
    }
    public void Minus()
    {
        {
            if (count > 0)
            {
                count--;
                price -= Sell.Instance.choiceSlot.inventoryItem.item.itemPrice;
            }
            else
                return;
            totalPrice.text = price.ToString();
            itemCount.text = count.ToString();
        }
    }
}
