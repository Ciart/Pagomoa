using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class AchieveContents : MonoBehaviour
    {
        [SerializeField] private MarketCondition marketCondition;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private Sprite up;
        [SerializeField] private Sprite down;
        [SerializeField] private GameObject updownImage;
        [SerializeField] int random;
        public List<InventorySlot> findallMineral;

        public void SetUI(Sprite s, int p)
        {
            image.sprite = s;
            price.text = p.ToString();
        }
        public void ChangePrice()
        {
            findallMineral = Achievements.Instance.AchieveMinerals.FindAll(a => a.GetSlotItem().type == ItemType.Mineral);
            for(int i = 0; i < findallMineral.Count; i++)
            {
                int save = findallMineral[i].GetSlotItem().price;
                random = Random.Range(-findallMineral[i].GetSlotItem().price, findallMineral[i].GetSlotItem().price);
                
                // NOTE: 
                // findallMineral[i].item.price += random;

                marketCondition.contentDatas[i].price.text = findallMineral[i].GetSlotItem().price.ToString();


                Sprite upordown;
                if (findallMineral[i].GetSlotItem().price > save)
                    upordown = up;
                else if (findallMineral[i].GetSlotItem().price < save)
                    upordown = down;
                else
                    return;

                marketCondition.contentDatas[i].updownImage.GetComponent<Image>().sprite = upordown;
            }
        }
    }
}
