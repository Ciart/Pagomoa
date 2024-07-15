using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class CountUI : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;

        [SerializeField] public TextMeshProUGUI itemCount;
        [SerializeField] public int count;

        private GameObject _choiceslot;

        private static CountUI instance;
        public static CountUI Instance
        {
            get
            {
                if (!instance)
                {
                    instance = GameObject.FindObjectOfType(typeof(CountUI)) as CountUI;
                }
                return instance;
            }
        }
        public void OnUI(GameObject obj)
        {
            transform.gameObject.SetActive(true);
            _choiceslot = obj;

            if (_choiceslot.GetComponent<BuySlot>())
            {
                _buttons[0].onClick.AddListener(BuyPlus);
                _buttons[1].onClick.AddListener(BuyMinus);
                _buttons[2].gameObject.SetActive(true);
                _buttons[3].gameObject.SetActive(true);
            }
            else if (_choiceslot.GetComponent<ShopSlot>())
            {
                _buttons[0].onClick.AddListener(SellPlus);
                _buttons[1].onClick.AddListener(SellMinus);
                _buttons[4].gameObject.SetActive(true);
                _buttons[5].gameObject.SetActive(true);
            }

            count = 1;
            itemCount.text = count.ToString();
        }
        public void OffUI()
        {
            ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
            ShopChat.Instance.CancleChat();
            if (_choiceslot.GetComponent<BuySlot>())
            {
                _buttons[0].onClick.RemoveAllListeners();
                _buttons[1].onClick.RemoveAllListeners();
            }
            else if (_choiceslot.GetComponent<ShopSlot>())
            {
                _buttons[0].onClick.RemoveAllListeners();
                _buttons[1].onClick.RemoveAllListeners();
            }

            for (int i = 2; i < 6; i++)
                _buttons[i].gameObject.SetActive(false);
            transform.gameObject.SetActive(false);
        }

        public void BuyPlus()
        {
            InventoryItem item = Buy.Instance.choiceSlot.inventoryItem;
            if (item.item.itemType == Item.ItemType.Use)
            {
                count++;
                ShopChat.Instance.TotalPriceToChat(count * item.item.itemPrice);
            }

            else if (item.item.itemType == Item.ItemType.Equipment || item.item.itemType == Item.ItemType.Inherent)
            {
                if (count < item.count)
                    count++;
                else
                    return;
            }
            else
                return;
            itemCount.text = count.ToString();
        }
        public void BuyMinus()
        {
            InventoryItem item = Buy.Instance.choiceSlot.inventoryItem;
            if (count > 1)
            {
                count--;
                ShopChat.Instance.TotalPriceToChat(count * item.item.itemPrice);
            }
            else
                return;
            itemCount.text = count.ToString();
        }
        public void BuySlots()
        {
            var Shop = Buy.Instance.choiceSlot.inventoryItem;
            if (Shop.item.itemType == Item.ItemType.Use)
            {
                if (GameManager.player.inventoryDB.Gold >= Shop.item.itemPrice * count && count > 0)
                {
                    GameManager.player.inventoryDB.Add(Shop.item, count);
                    AuctionDB.Instance.Remove(Shop.item);
                    ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
                    OffUI();
                }
                else
                    return;

                ShopChat.Instance.ThankChat();
            }

            else if (Shop.item.itemType == Item.ItemType.Equipment || Shop.item.itemType == Item.ItemType.Inherent)
            {
                if (GameManager.player.inventoryDB.Gold >= Shop.item.itemPrice && Shop.count == count)
                {
                    GameManager.player.inventoryDB.Add(Shop.item, 0);
                    AuctionDB.Instance.Remove(Shop.item);
                    Buy.Instance.UpdateCount();
                    Buy.Instance.SoldOut();
                    ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
                    OffUI();
                }
                else
                    return;
                ShopChat.Instance.ThankChat();
            }
        }
        public void SellPlus()
        {
            InventoryItem item = Sell.Instance.choiceSlot.inventoryItem;
            if (count < item.count)
            {
                count++;
                ShopChat.Instance.TotalPriceToChat(count * item.item.itemPrice);
            }
            else
                return;
            itemCount.text = count.ToString();
        }
        public void SellMinus()
        {
            InventoryItem item = Sell.Instance.choiceSlot.inventoryItem;
            if (count > 1)
            {
                count--;
                ShopChat.Instance.TotalPriceToChat(count * item.item.itemPrice);
            }
            else
                return;
            itemCount.text = count.ToString();
        }
        public void SellSlots()
        {
            for (int i = 0; i < count; i++)
            {
                if (Sell.Instance.choiceSlot.inventoryItem.count > 1)
                {
                    GameManager.player.inventoryDB.SellItem(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                else if (Sell.Instance.choiceSlot.inventoryItem.count == 1)
                {
                    GameManager.player.inventoryDB.SellItem(Sell.Instance.choiceSlot.inventoryItem.item);
                    //QuickSlotItemDB.instance.CleanSlot(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                Sell.Instance.DeleteSlot();
                Sell.Instance.ResetSlot();
            }
            count = 1;
            itemCount.text = count.ToString();
            ShopUIManager.Instance.hovering.boostImage.sprite = ShopUIManager.Instance.hovering.hoverImage[1];
            OffUI();
            ShopChat.Instance.ThankChat();
        }
    }
}
