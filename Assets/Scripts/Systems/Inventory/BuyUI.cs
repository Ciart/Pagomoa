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
        [SerializeField] private List<BuyArtifactSlotUI> inherentSlots = new List<BuyArtifactSlotUI>();

        private void Awake()
        {
            MakeBuyUISlot();
        }
        private void OnEnable()
        {
            UIManager.instance.shopUI.shopGoldUI.text = Game.instance.player.inventory.gold.ToString();
        }
        public void MakeBuyUISlot()
        {
            var itemIDs = UIManager.instance.shopUI.GetShopItemIDs();
            
            for (int i = 0; i < itemIDs.Count; i++)
            {
                var item = ResourceSystem.Instance.GetItem(itemIDs[i]);
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
                else if (item.type == ItemType.Inherent)
                {
                    var spawnedSlot = Instantiate(instanceArtifactSlotUI, artifactPanel.transform);
                    
                    spawnedSlot.SetSlotID(i);
                    spawnedSlot.gameObject.SetActive(true);
                    inherentSlots.Add(spawnedSlot);
                }
            }
            SetItemToBuySlot();
        }
        public void SetItemToBuySlot()
        {
            var shopUI = Game.Instance.UI.shopUI;
            
            foreach (var artifact in artifactSlots)
            {
                var slot = new Slot();
                slot.SetSlotItemID(shopUI.GetShopItemIDs()[artifact.GetSlotID()]);
                artifact.SetSlot(slot);
                
                slot = null;
            }
            
            foreach (var use in consumptionSlots)
            {
                var slot = new Slot();
                slot.SetSlotItemID(shopUI.GetShopItemIDs()[use.GetSlotID()]);
                use.SetSlot(slot);
                
                slot = null;
            }

            foreach (var inherent in inherentSlots)
            {
                var slot = new Slot();
                slot.SetSlotItemID(shopUI.GetShopItemIDs()[inherent.GetSlotID()]);
                inherent.SetSlot(slot);
                
                slot = null;
            }
            
            UpdateBuyUISlot();
        }
        public void UpdateBuyUISlot()
        {
            var shopUI = UIManager.instance.shopUI;
            
            var equipIndex = 0;
            var useIndex = 0;
            var inherentIndex = 0;
            
            foreach (var id in shopUI.GetShopItemIDs())
            {
                var item = ResourceSystem.Instance.GetItem(id);
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
                else if (item.type == ItemType.Inherent)
                {
                    inherentSlots[inherentIndex].slotImage.sprite = _papersSprites[equipIndex];
                    inherentSlots[inherentIndex].soldOutImage.sprite = _soldOutsSprites[equipIndex];
                    equipIndex++;
                    inherentIndex++;
                }
            }
        }
        
        public void SoldOut(int slotID)
        {
            foreach (var artifact in artifactSlots)
            {
                if (artifact.GetSlotID() == slotID)
                {
                    artifact.soldOutImage.gameObject.SetActive(true);
                    artifact.buyCheckButton.interactable = false;
                }
            }

            foreach (var inherent in inherentSlots)
            {
                if (inherent.GetSlotID() == slotID)
                {
                    inherent.soldOutImage.gameObject.SetActive(true);
                    inherent.buyCheckButton.interactable = false;
                }
            }
        }
    }
}
