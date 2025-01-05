using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuyUI : MonoBehaviour
    {
        [SerializeField] private GameObject artifactPanel;
        [SerializeField] private BuyArtifactSlot instanceArtifactSlot;
        [SerializeField] private GameObject consumableItemsPanel;
        [SerializeField] private BuySlot instanceBuySlot;
        
        [SerializeField] private Sprite[] _papersSprites;
        [SerializeField] private Sprite[] _soldOutsSprites;
        
        [Header("Run Time UI Can Be None")]
        [SerializeField] private List<BuyArtifactSlot> artifactSlots = new List<BuyArtifactSlot>();
        [SerializeField] private List<BuySlot> consumptionSlots = new List<BuySlot>();

        private void Awake()
        {
            MakeBuyUISlot();
        }
        private void OnEnable()
        {
            UIManager.instance.shopUI.shopGoldUI.text = GameManager.instance.player.inventory.gold.ToString();
        }
        public void MakeBuyUISlot()
        {
            var shopUI = UIManager.instance.shopUI;
            
            for (int i = 0; i < shopUI.GetShopItems().Count; i++)
            {
                if (shopUI.GetShopItems()[i].type == ItemType.Equipment)
                {
                    var spawnedSlot = Instantiate(instanceArtifactSlot, artifactPanel.transform);
                    spawnedSlot.GetSlotItem().type = ItemType.Equipment;
                    artifactSlots.Add(spawnedSlot);
                    spawnedSlot.gameObject.SetActive(true);
                }
                else if (shopUI.GetShopItems()[i].type == ItemType.Use)
                {
                    var spawnedSlot = Instantiate(instanceBuySlot, consumableItemsPanel.transform);
                    consumptionSlots.Add(spawnedSlot);
                    spawnedSlot.gameObject.SetActive(true);
                }
            }
            SetItemToBuySlot();
        }
        public void SetItemToBuySlot()
        {
            var shopUI = UIManager.instance.shopUI;
            
            int equipIndex = 0;
            int useIndex = 0;

            foreach (var shopItem in shopUI.GetShopItems())
            {
                if (shopItem.type == ItemType.Equipment)
                {
                    artifactSlots[equipIndex].SetSlotItem(shopItem);
                    equipIndex++;
                }
                else if (shopItem.type == ItemType.Use)
                {
                    consumptionSlots[useIndex].SetSlotItem(shopItem);
                    
                    useIndex++;
                }
            }
            
            UpdateBuyUISlot();
        }
        public void UpdateBuyUISlot()
        {
            var shopUI = UIManager.instance.shopUI;
            
            int equipIndex = 0;
            int useIndex = 0;
            foreach (var shopItem in shopUI.GetShopItems())
            {
                if (shopItem.type == ItemType.Equipment)
                {
                    artifactSlots[equipIndex].GetComponent<Image>().sprite = _papersSprites[equipIndex];
                    artifactSlots[equipIndex].soldOut.GetComponent<Image>().sprite = _soldOutsSprites[equipIndex];
                    artifactSlots[equipIndex].UpdateBuySlot();
                    equipIndex++;
                }
                else if (shopItem.type == ItemType.Use)
                {
                    if (useIndex < 3)
                    {
                        consumptionSlots[useIndex].GetComponent<Image>().sprite = _papersSprites[useIndex + 3];
                    }
                    else if (useIndex is > 3 and < 6)
                    {
                        consumptionSlots[useIndex].GetComponent<Image>().sprite = _papersSprites[useIndex];
                    }
                    else if (useIndex >= 6)
                    {
                        consumptionSlots[useIndex].GetComponent<Image>().sprite = _papersSprites[useIndex - 3];
                    }
                    consumptionSlots[useIndex].UpdateBuySlot();
                    useIndex++;
                }
            }
        }
        
        public void SoldOut()
        {
            var artifactSlot = UIManager.instance.shopUI.chosenSlot as BuyArtifactSlot;
            
            artifactSlot.artifactCount.text = artifactSlot.GetSlotItemCount().ToString();
            
            artifactSlot.soldOut.SetActive(true);
            artifactSlot.artifactSlotButton.interactable = false;
        }
    }
}
