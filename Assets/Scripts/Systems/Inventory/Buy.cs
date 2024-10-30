using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Buy : MonoBehaviour
    {
        public BuySlot chosenBuySlot;
        public InventorySlotUI chosenSellSlot;
        public TextMeshProUGUI countUIText;
        public int countUINum;

        private const int ParentNum = 3;
        private const int ChildNum = 3;
        private List<InventorySlotUI> _slotData = new List<InventorySlotUI>();

        [SerializeField] private GameObject[] slotsParent = new GameObject[ParentNum];
        [SerializeField] private GameObject[] slot = new GameObject[ChildNum];
        [SerializeField] private List<BuyArtifactSlot> _artifactSlots = new List<BuyArtifactSlot>();
        [SerializeField] private List<BuySlot> _consumptionSlots = new List<BuySlot>();
        [SerializeField] private Sprite[] _papersSprites;
        [SerializeField] private Sprite[] _soldOutsSprites;
        [SerializeField] private Sprite _emptyImage;
        [SerializeField] private GameObject _countUI;
        [SerializeField] private Button[] _countUIBtns;
        private GameObject _choiceSlot;

        private static Buy instance;
        public static Buy Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType(typeof(Buy)) as Buy;
                }
                return instance;
            }
        }
        private void Awake()
        {
            MakeBuyUISlot();
            MakeSellUISlot();
        }
        private void OnEnable()
        {
            transform.GetComponentInParent<ShopUIManager>().gold[0].text = GameManager.instance.player.inventory.Gold.ToString();
            DeleteSellUISlot();
            ResetSellUISlot();
        }
        public void MakeBuyUISlot()
        {
            for (int i = 0; i < AuctionDB.Instance.auctionItem.Count; i++)
            {
                if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Equipment)
                {
                    var spawnedSlot = Instantiate(slot[0], slotsParent[0].transform);
                    _artifactSlots.Add(spawnedSlot.GetComponent<BuyArtifactSlot>());
                    spawnedSlot.SetActive(true);
                }
                else if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Use)
                {
                    var spawnedSlot = Instantiate(slot[1], slotsParent[1].transform);
                    _consumptionSlots.Add(spawnedSlot.GetComponent<BuySlot>());
                    spawnedSlot.SetActive(true);
                }
            }
            ResetBuyUISlot();
        }
        public void ResetBuyUISlot()
        {
            int j = 0;
            int z = 0;
            for (int i = 0; i < AuctionDB.Instance.auctionItem.Count; i++)
            {
                if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Equipment)
                {
                    _artifactSlots[j].slot = AuctionDB.Instance.auctionItem[i];
                    j++;
                }
                else if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Use)
                {
                    _consumptionSlots[z].slot = AuctionDB.Instance.auctionItem[i];
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
            for (int i = 0; i < AuctionDB.Instance.auctionItem.Count; i++)
            {
                if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Equipment)
                {
                    _artifactSlots[j].UpdateArtifactSlot();
                    _artifactSlots[j].GetComponent<Image>().sprite = _papersSprites[j];
                    _artifactSlots[j]._soldOut.GetComponent<Image>().sprite = _soldOutsSprites[j];
                    j++;
                }
                else if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Use)
                {
                    _consumptionSlots[z].UpdateConsumptionSlot();
                    if (z < 3)
                    {
                        _consumptionSlots[z].GetComponent<Image>().sprite = _papersSprites[z + 3];
                    }
                    else if (z >= 3 && z < 6)
                    {
                        _consumptionSlots[z].GetComponent<Image>().sprite = _papersSprites[z];
                    }
                    else if (z >= 6)
                    {
                        _consumptionSlots[z].GetComponent<Image>().sprite = _papersSprites[z - 3];
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
            for (int i = 0; i < _artifactSlots.Count; i++)
                Destroy(_artifactSlots[i].gameObject);
            _artifactSlots.Clear();
        }
        public void SoldOut()
        {
            chosenBuySlot.GetComponent<BuyArtifactSlot>()._soldOut.SetActive(true);
            chosenBuySlot.GetComponent<Button>().interactable = false;
        }
        public void UpdateCount()
        {
            chosenBuySlot.GetComponent<BuyArtifactSlot>().itemNum.text = chosenBuySlot.slot.count.ToString();
        }
        public void OnCountUI(GameObject obj)
        {
            _countUI.SetActive(true);
            _choiceSlot = obj;

            if (_choiceSlot.GetComponent<BuySlot>())
            {
                _countUIBtns[0].onClick.AddListener(BuyPlus);
                _countUIBtns[1].onClick.AddListener(BuyMinus);
                _countUIBtns[2].gameObject.SetActive(true);
                _countUIBtns[3].gameObject.SetActive(true);
            }
            else if (_choiceSlot.GetComponent<ShopSlot>())
            {
                _countUIBtns[0].onClick.AddListener(SellPlus);
                _countUIBtns[1].onClick.AddListener(SellMinus);
                _countUIBtns[4].gameObject.SetActive(true);
                _countUIBtns[5].gameObject.SetActive(true);
            }

            countUINum = 1;
            countUIText.text = countUINum.ToString();
        }
        public void OffCountUI()
        {
            ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
            ShopChat.Instance.CancleChat();
            if (_choiceSlot.GetComponent<BuySlot>())
            {
                _countUIBtns[0].onClick.RemoveAllListeners();
                _countUIBtns[1].onClick.RemoveAllListeners();
            }
            else if (_choiceSlot.GetComponent<ShopSlot>())
            {
                _countUIBtns[0].onClick.RemoveAllListeners();
                _countUIBtns[1].onClick.RemoveAllListeners();
            }

            for (int i = 2; i < 6; i++)
                _countUIBtns[i].gameObject.SetActive(false);
            _countUI.SetActive(false);
        }

        public void BuyPlus()
        {
            InventorySlot item = Buy.Instance.chosenBuySlot.slot;
            if (item.item.itemType == Item.ItemType.Use)
            {
                countUINum++;
                ShopChat.Instance.TotalPriceToChat(countUINum * item.item.itemPrice);
            }

            else if (item.item.itemType == Item.ItemType.Equipment || item.item.itemType == Item.ItemType.Inherent)
            {
                if (countUINum < item.count)
                    countUINum++;
                else
                    return;
            }
            else
                return;
            countUIText.text = countUINum.ToString();
        }
        public void BuyMinus()
        {
            InventorySlot item = chosenBuySlot.slot;
            if (countUINum > 1)
            {
                countUINum--;
                ShopChat.Instance.TotalPriceToChat(countUINum * item.item.itemPrice);
            }
            else
                return;
            countUIText.text = countUINum.ToString();
        }
        public void BuySlots()
        {
            var Shop = chosenBuySlot.slot;
            if (Shop.item.itemType == Item.ItemType.Use)
            {
                if (GameManager.instance.player.inventory.Gold >= Shop.item.itemPrice * countUINum && countUINum > 0)
                {
                    GameManager.instance.player.inventory.Add(Shop.item, countUINum);
                    AuctionDB.Instance.Remove(Shop.item);
                    ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
                    OffCountUI();
                    Debug.Log("호출");

                }
                else
                    return;

                ShopChat.Instance.ThankChat();
            }

            else if (Shop.item.itemType == Item.ItemType.Equipment || Shop.item.itemType == Item.ItemType.Inherent)
            {
                if (GameManager.instance.player.inventory.Gold >= Shop.item.itemPrice && Shop.count == countUINum)
                {
                    GameManager.instance.player.inventory.Add(Shop.item, 0);
                    AuctionDB.Instance.Remove(Shop.item);
                    UpdateCount();
                    SoldOut();
                    ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
                    OffCountUI();
                }
                else
                    return;
                ShopChat.Instance.ThankChat();
            }
        }
        public void SellPlus()
        {
            InventorySlot item = chosenSellSlot.slot;
            if (countUINum < item.count)
            {
                countUINum++;
                ShopChat.Instance.TotalPriceToChat(countUINum * item.item.itemPrice);
            }
            else
                return;
            countUIText.text = countUINum.ToString();
        }
        public void SellMinus()
        {
            InventorySlot item = chosenSellSlot.slot;
            if (countUINum > 1)
            {
                countUINum--;
                ShopChat.Instance.TotalPriceToChat(countUINum * item.item.itemPrice);
            }
            else
                return;
            countUIText.text = countUINum.ToString();
        }
        public void SellSlots()
        {
            for (int i = 0; i < countUINum; i++)
            {
                if (chosenSellSlot.slot.count > 1)
                {
                    GameManager.instance.player.inventory.SellItem(chosenSellSlot.slot.item);
                }
                else if (chosenSellSlot.slot.count == 1)
                {
                    GameManager.instance.player.inventory.SellItem(chosenSellSlot.slot.item);
                    //QuickSlotItemDB.instance.CleanSlot(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                DeleteSellUISlot();
                ResetSellUISlot();
            }
            countUINum = 1;
            countUIText.text = countUINum.ToString();
            ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
            OffCountUI();
            ShopChat.Instance.ThankChat();
        }
    }
}
