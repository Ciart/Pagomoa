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
        [SerializeField] private BuyArtifactSlotUI instanceArtifactSlotUI;
        [SerializeField] private GameObject consumableItemsPanel;
        [SerializeField] private BuySlotUI instanceBuySlotUI;
        
        [SerializeField] private Sprite[] _papersSprites;
        [SerializeField] private Sprite[] _soldOutsSprites;
        
        [Header("Run Time UI Can Be None")]
        [SerializeField] private List<BuyArtifactSlotUI> artifactSlots = new List<BuyArtifactSlotUI>();
        [SerializeField] private List<BuySlotUI> consumptionSlots = new List<BuySlotUI>();

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
            var itemIDs = UIManager.instance.shopUI.GetShopItemIDs();
            
            for (int i = 0; i < itemIDs.Count; i++)
            {
                var item = ResourceSystem.instance.GetItem(itemIDs[i]);
                if (item.type == ItemType.Equipment)
                {
                    var spawnedSlot = Instantiate(instanceArtifactSlotUI, artifactPanel.transform);
                    
                    spawnedSlot.SetSlotID(i);
                    spawnedSlot.gameObject.SetActive(true);
                    artifactSlots.Add(spawnedSlot);
                }
                else if (item.type == ItemType.Use)
                {
                    var spawnedSlot = Instantiate(instanceBuySlotUI, consumableItemsPanel.transform);
                    
                    spawnedSlot.SetSlotID(i);
                    spawnedSlot.gameObject.SetActive(true);
                    consumptionSlots.Add(spawnedSlot);
                }
            }
            SetItemToBuySlot();
        }
        public void SetItemToBuySlot()
        {
            var shopUI = UIManager.instance.shopUI;
            
            foreach (var artifact in artifactSlots)
            {
                var slot = new Slot();
                slot.SetSlotItemID(shopUI.GetShopItemIDs()[artifact.slotID]);
                artifact.SetSlot(slot);
                
                slot = null;
            }
            
            foreach (var use in consumptionSlots)
            {
                var slot = new Slot();
                slot.SetSlotItemID(shopUI.GetShopItemIDs()[use.slotID]);
                use.SetSlot(slot);
                
                slot = null;
            }
            
            UpdateBuyUISlot();
        }
        public void UpdateBuyUISlot()
        {
            var shopUI = UIManager.instance.shopUI;
            
            int equipIndex = 0;
            int useIndex = 0;
            
            foreach (var id in shopUI.GetShopItemIDs())
            {
                var item = ResourceSystem.instance.GetItem(id);
                if (item.type == ItemType.Equipment)
                {
                    artifactSlots[equipIndex].slotImage.sprite = _papersSprites[equipIndex];
                    artifactSlots[equipIndex].soldOutImage.sprite = _soldOutsSprites[equipIndex];
                    equipIndex++;
                }
                else if (item.type == ItemType.Use)
                {
                    if (useIndex < 3)
                    {
                        consumptionSlots[useIndex].slotImage.sprite = _papersSprites[useIndex + 3];
                    }
                    else if (useIndex is > 3 and < 6)
                    {
                        consumptionSlots[useIndex].slotImage.sprite = _papersSprites[useIndex];
                    }
                    else if (useIndex >= 6)
                    {
                        consumptionSlots[useIndex].slotImage.sprite = _papersSprites[useIndex - 3];
                    }
                    useIndex++;
                }
            }
        }
        
        public void SoldOut(int slotID)
        {
            var itemIDList = UIManager.instance.shopUI.GetShopItemIDs();
            var item = ResourceSystem.instance.GetItem(itemIDList[slotID]);
            if (item.type != ItemType.Equipment || item.type != ItemType.Inherent) return;
            
            foreach (var artifact in artifactSlots)
            {
                if (artifact.slotID == slotID)
                {
                    artifact.soldOutImage.gameObject.SetActive(true);
                    artifact.artifactSlotButton.interactable = false;
                }
            }
        }
    }
}
