using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AchieveContents : MonoBehaviour
{
    [SerializeField] private MarketCondition marketCondition;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;
    [SerializeField] private GameObject updownImage;
    [SerializeField] int random;
    public List<InventoryItem> findallMineral;

    public void SetUI(Sprite s, int p)
    {
        image.sprite = s;
        price.text = p.ToString();
    }
    public void ChangePrice()
    {
        findallMineral = Achievements.Instance.AchieveMinerals.FindAll(a => a.item.itemType == Item.ItemType.Mineral);
        for(int i = 0; i < findallMineral.Count; i++)
        {
            int save = findallMineral[i].item.itemPrice;
            random = Random.Range(-findallMineral[i].item.itemPrice, findallMineral[i].item.itemPrice);
            findallMineral[i].item.itemPrice += random;

            marketCondition.contentDatas[i].price.text = findallMineral[i].item.itemPrice.ToString();

            if (findallMineral[i].item.itemPrice > save)
            {
                marketCondition.contentDatas[i].updownImage.GetComponent<Image>().sprite = up;
            }
            else if(findallMineral[i].item.itemPrice < save)
            {
                marketCondition.contentDatas[i].updownImage.GetComponent<Image>().sprite = down;
            }
        }
    }
}
