using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Buy : MonoBehaviour
    {
        [SerializeField] public BuySlot choiceSlot;
        [SerializeField] private GameObject _underMask;
        [SerializeField] private GameObject _artifactSlotsParent;
        [SerializeField] private GameObject _consumptionSlotsParent;
        [SerializeField] private GameObject _artifactSlot;
        [SerializeField] private GameObject _consumptionSlot;
        [SerializeField] public TextMeshProUGUI gold;
        [SerializeField] private List<BuySlot> _artifactSlots = new List<BuySlot>();
        [SerializeField] private List<BuySlot> _consumptionSlots = new List<BuySlot>();
        [SerializeField] private Sprite[] _papers;
        [SerializeField] private Sprite[] _soldOuts;


        private static Buy instance;
        public static Buy Instance
        {
            get
            {
                if (!instance)
                {
                    instance = GameObject.FindObjectOfType(typeof(Buy)) as Buy;
                }
                return instance;
            }
        }
        private void Awake()
        {
            MakeSlot();
            //MakeUnderMask();
        }
        public void MakeSlot()
        {
            for (int i = 0; i < AuctionDB.Instance.auctionItem.Count; i++)
            {
                if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Equipment)
                {
                    GameObject SpawnedSlot = Instantiate(_artifactSlot, _artifactSlotsParent.transform);
                    _artifactSlots.Add(SpawnedSlot.GetComponent<BuySlot>());
                    SpawnedSlot.SetActive(true);
                }
                else if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Use)
                {
                    GameObject SpawnedSlot = Instantiate(_consumptionSlot, _consumptionSlotsParent.transform);
                    _consumptionSlots.Add(SpawnedSlot.GetComponent<BuySlot>());
                    SpawnedSlot.SetActive(true);
                }
            }
            ResetSlot();
        }
        public void ResetSlot()
        {
            int j = 0;
            int z = 0;
            for (int i = 0; i < AuctionDB.Instance.auctionItem.Count; i++)
            {
                if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Equipment)
                {
                    _artifactSlots[j].inventoryItem = AuctionDB.Instance.auctionItem[i];
                    j++;
                }
                else if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Use)
                {
                    _consumptionSlots[z].inventoryItem = AuctionDB.Instance.auctionItem[i];
                    z++;
                }
                else
                    return;
            }
            UpdateSlot();
        }
        public void UpdateSlot()
        {
            int j = 0;
            int z = 0;
            for (int i = 0; i < AuctionDB.Instance.auctionItem.Count; i++)
            {
                if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Equipment)
                {
                    _artifactSlots[j].UpdateArtifactSlot();
                    _artifactSlots[j].GetComponent<Image>().sprite = _papers[j];
                    _artifactSlots[j]._soldOut.GetComponent<Image>().sprite = _soldOuts[j];
                    j++;
                }
                else if (AuctionDB.Instance.auctionItem[i].item.itemType == Item.ItemType.Use)
                {
                    _consumptionSlots[z].UpdateConsumptionSlot();
                    if (z < 3)
                    {
                        _consumptionSlots[z].GetComponent<Image>().sprite = _papers[z + 3];
                    }
                    else if (z >= 3 && z < 6)
                    {
                        _consumptionSlots[z].GetComponent<Image>().sprite = _papers[z];
                    }
                    else if (z >= 6)
                    {
                        _consumptionSlots[z].GetComponent<Image>().sprite = _papers[z - 3];
                    }
                    z++;
                }
            }
            gold.text = InventoryDB.Instance.Gold.ToString();
        }
        public void DestroySlot()
        {
            for (int i = 0; i < _artifactSlots.Count; i++)
                Destroy(_artifactSlots[i].gameObject);
            _artifactSlots.Clear();
        }
        public void SoldOut()
        {
            choiceSlot._soldOut.SetActive(true);
            choiceSlot.GetComponent<Button>().interactable = false;
        }
        public void UpdateCount()
        {
            choiceSlot.itemNum.text = choiceSlot.inventoryItem.count.ToString();
        }
        private void MakeUnderMask()
        {
            Vector3 position = _consumptionSlotsParent.transform.position;
            GameObject mask = Instantiate(_underMask, transform.GetChild(0));
            mask.transform.position = position;
            mask.SetActive(true);
        }
    }
}
