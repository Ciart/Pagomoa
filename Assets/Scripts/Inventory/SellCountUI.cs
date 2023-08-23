using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellCountUI : MonoBehaviour
{
    [SerializeField] private Image item;
    [SerializeField] public TextMeshProUGUI itemCount;
    [SerializeField] public TextMeshProUGUI totalPrice;
    [SerializeField] public int price = 0;
    [SerializeField] public int count = 0;
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
        item.sprite = image;
    }
    public void Plus()
    {
        if (count < EtcInventory.Instance.choiceSlot.inventoryItem.count)
        {
            count++;
            price += EtcInventory.Instance.choiceSlot.inventoryItem.item.itemPrice;
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
                price -= EtcInventory.Instance.choiceSlot.inventoryItem.item.itemPrice;
            }
            else
                return;
            totalPrice.text = price.ToString();
            itemCount.text = count.ToString();
        }
    }
}
