using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class BuyUI : MonoBehaviour
    {
        public BuySlot chosenBuySlot;
        public InventorySlotUI chosenSellSlot;

        private const int ParentNum = 3;
        private const int ChildNum = 3;
        private List<InventorySlotUI> _slotData = new List<InventorySlotUI>();

        [SerializeField] private GameObject[] slotsParent = new GameObject[ParentNum];
        [SerializeField] private GameObject[] slot = new GameObject[ChildNum];
        
        [SerializeField] private Sprite[] _papersSprites;
        [SerializeField] private Sprite[] _soldOutsSprites;
        [SerializeField] private Sprite _emptyImage;
        
        [Header("Run Time UI Can Be None")]
        [SerializeField] private List<BuyArtifactSlot> artifactSlots = new List<BuyArtifactSlot>();
        [SerializeField] private List<BuySlot> consumptionSlots = new List<BuySlot>();
        
        private GameObject _choiceSlot;


        private void Awake()
        {
            MakeBuyUISlot();
            MakeSellUISlot(); 
        }
        private void OnEnable()
        {
            UIManager.instance.shopUI.playerGold[0].text = GameManager.instance.player.inventory.Gold.ToString();
            DeleteSellUISlot();
            ResetSellUISlot();
        }
        public void MakeBuyUISlot()
        {
            for (int i = 0; i < AuctionDB.Instance.auctionItems.Count; i++)
            {
                if (AuctionDB.Instance.auctionItems[i].item.type == ItemType.Equipment)
                {
                    var spawnedSlot = Instantiate(slot[0], slotsParent[0].transform);
                    artifactSlots.Add(spawnedSlot.GetComponent<BuyArtifactSlot>());
                    spawnedSlot.SetActive(true);
                }
                else if (AuctionDB.Instance.auctionItems[i].item.type == ItemType.Use)
                {
                    var spawnedSlot = Instantiate(slot[1], slotsParent[1].transform);
                    consumptionSlots.Add(spawnedSlot.GetComponent<BuySlot>());
                    spawnedSlot.SetActive(true);
                }
            }
            ResetBuyUISlot();
        }
        public void ResetBuyUISlot()
        {
            int j = 0;
            int z = 0;
            for (int i = 0; i < AuctionDB.Instance.auctionItems.Count; i++)
            {
                if (AuctionDB.Instance.auctionItems[i].item.type == ItemType.Equipment)
                {
                    artifactSlots[j].slot = AuctionDB.Instance.auctionItems[i];
                    j++;
                }
                else if (AuctionDB.Instance.auctionItems[i].item.type == ItemType.Use)
                {
                    consumptionSlots[z].slot = AuctionDB.Instance.auctionItems[i];
                    z++;
                }
                else
                    return;
            }
            UpdateBuyUISlot();
        }
        public void UpdateBuyUISlot()
        {
            int j = 0;
            int z = 0;
            for (int i = 0; i < AuctionDB.Instance.auctionItems.Count; i++)
            {
                if (AuctionDB.Instance.auctionItems[i].item.type == ItemType.Equipment)
                {
                    artifactSlots[j].UpdateArtifactSlot();
                    artifactSlots[j].GetComponent<Image>().sprite = _papersSprites[j];
                    artifactSlots[j].soldOut.GetComponent<Image>().sprite = _soldOutsSprites[j];
                    j++;
                }
                else if (AuctionDB.Instance.auctionItems[i].item.type == ItemType.Use)
                {
                    consumptionSlots[z].UpdateConsumptionSlot();
                    if (z < 3)
                    {
                        consumptionSlots[z].GetComponent<Image>().sprite = _papersSprites[z + 3];
                    }
                    else if (z >= 3 && z < 6)
                    {
                        consumptionSlots[z].GetComponent<Image>().sprite = _papersSprites[z];
                    }
                    else if (z >= 6)
                    {
                        consumptionSlots[z].GetComponent<Image>().sprite = _papersSprites[z - 3];
                    }
                    z++;
                }
            }
        }
        public void MakeSellUISlot()
        {
            for(int i = 0; i < GameManager.instance.player.inventory.items.Length; i++)
            {
                var spawnedSlot = Instantiate(slot[2], slotsParent[2].transform);
                _slotData.Add(spawnedSlot.GetComponent<InventorySlotUI>());
                _slotData[i].id = i;
                spawnedSlot.SetActive(true);
            }
            ResetSellUISlot();
        }
        public void ResetSellUISlot()
        {
            for(int i = 0; i < _slotData.Count; i++)
                _slotData[i].slot = GameManager.instance.player.inventory.items[i];
            UpdateSellUISlot();
        }
        public void UpdateSellUISlot()
        {
            for (int i = 0; i < GameManager.instance.player.inventory.items.Length; i++)
            {
                _slotData[i].SetItem(GameManager.instance.player.inventory.items[i]);
            }
        }
        public void DeleteSellUISlot()
        {
            if (GameManager.instance.player.inventory.items.Length >= 0)
            {
                for (int i = 0; i < _slotData.Count; i++)
                    _slotData[i].ResetItem();
            }
        }
        public void DestroySlot()
        {
            for (int i = 0; i < artifactSlots.Count; i++)
                Destroy(artifactSlots[i].gameObject);
            artifactSlots.Clear();
        }
        public void SoldOut()
        {
            chosenBuySlot.GetComponent<BuyArtifactSlot>().soldOut.SetActive(true);
            chosenBuySlot.GetComponent<Button>().interactable = false;
        }
        public void UpdateCount()
        {
            chosenBuySlot.GetComponent<BuyArtifactSlot>().itemNum.text = chosenBuySlot.slot.count.ToString();
        }
    }
}
