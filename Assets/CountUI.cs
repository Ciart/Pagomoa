using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class CountUI : MonoBehaviour
    {
        public TextMeshProUGUI countUIText;
        public int inputCount;
        
        private SlotType _chosenSlotType;
        
        [Header("아이템 구매&판매 제어 버튼")]
        public Button countUpButton;
        public Button countDownButton;
        [SerializeField] private Button buyYesButton;
        [SerializeField] private Button buyNoButton;
        [SerializeField] private Button sellYesButton;
        [SerializeField] private Button sellNoButton;
        
        public void ActiveCountUI(SlotType type)
        {
            _chosenSlotType = type;
            
            countUpButton.gameObject.SetActive(true);
            countDownButton.gameObject.SetActive(true);
            
            if (type == SlotType.Buy)
            {
                countUpButton.onClick.AddListener(BuyPlus);
                countDownButton.onClick.AddListener(BuyMinus);
                buyYesButton.gameObject.SetActive(true);
                buyNoButton.gameObject.SetActive(true);
            }
            else if (type == SlotType.Shop)
            {
                countUpButton.onClick.AddListener(SellPlus);
                countDownButton.onClick.AddListener(SellMinus);
                sellYesButton.gameObject.SetActive(true);
                sellNoButton.gameObject.SetActive(true);
            }

            inputCount = 1;
            countUIText.text = inputCount.ToString();
        }
        
        
        public void DisableCountUI()
        {
            var shopUI = UIManager.instance.shopUI;
            shopUI.hovering.boostImage.sprite = shopUI.hovering.hoverImage[1];
            shopUI.GetShopChat().CancelChat();
            
            if (_chosenSlotType == SlotType.Buy)
            {
                countUpButton.onClick.RemoveAllListeners();
                countDownButton.onClick.RemoveAllListeners();
            }
            else if (_chosenSlotType == SlotType.Shop)
            {
                countUpButton.onClick.RemoveAllListeners();
                countDownButton.onClick.RemoveAllListeners();
            }

            countUpButton.gameObject.SetActive(false);
            countDownButton.gameObject.SetActive(false);
            buyYesButton.gameObject.SetActive(false);
            buyNoButton.gameObject.SetActive(false);
            sellYesButton.gameObject.SetActive(false);
            sellNoButton.gameObject.SetActive(false);
            
            gameObject.SetActive(false);
        }
        
        public void BuyPlus()
        {
            var shopUI = UIManager.instance.shopUI;
            var buySlot = shopUI.GetBuyUI().chosenBuySlot.slot;
            
            if (buySlot.item.itemType == Item.ItemType.Use)
            {
                inputCount++;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * buySlot.item.itemPrice);
            }

            else if (buySlot.item.itemType == Item.ItemType.Equipment || buySlot.item.itemType == Item.ItemType.Inherent)
            {
                if (inputCount < buySlot.count)
                    inputCount++;
            }
            
            countUIText.text = inputCount.ToString();
        }
        
        public void BuyMinus()
        {
            var shopUI = UIManager.instance.shopUI;
            var buySlot = shopUI.GetBuyUI().chosenBuySlot.slot;
            
            if (inputCount > 1)
            {
                inputCount--;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * buySlot.item.itemPrice);
            }
            
            countUIText.text = inputCount.ToString();
        }
        public void BuySlots()
        {
            var inventory = GameManager.instance.player.inventory;
            var shopUI = UIManager.instance.shopUI;
            var buySlot = shopUI.GetBuyUI().chosenBuySlot.slot;
            
            if (buySlot.item.itemType == Item.ItemType.Use)
            {
                if (inventory.Gold >= buySlot.item.itemPrice * inputCount && inputCount > 0)
                {
                    inventory.Add(buySlot.item, inputCount);
                    AuctionDB.Instance.Remove(buySlot.item);
                    shopUI.hovering.boostImage.sprite = shopUI.hovering.hoverImage[1];
                    DisableCountUI();
                }
                else
                    return;

                shopUI.GetShopChat().ThankChat();
            }
            else if (buySlot.item.itemType == Item.ItemType.Equipment || buySlot.item.itemType == Item.ItemType.Inherent)
            {
                if (inventory.Gold >= buySlot.item.itemPrice && buySlot.count == inputCount)
                {
                    inventory.Add(buySlot.item, 0);
                    AuctionDB.Instance.Remove(buySlot.item);
                    
                    shopUI.hovering.boostImage.sprite = shopUI.hovering.hoverImage[1];
                    DisableCountUI();
                    
                    UIManager.instance.shopUI.GetBuyUI().UpdateCount();
                    UIManager.instance.shopUI.GetBuyUI().SoldOut();
                }
                else
                    return;
                shopUI.GetShopChat().ThankChat();
            }
        }
        
        public void SellPlus()
        {
            var shopUI = UIManager.instance.shopUI;
            var sellSlot = UIManager.instance.shopUI.GetBuyUI().chosenSellSlot.slot;
            
            if (inputCount < sellSlot.count)
            {
                inputCount++;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * sellSlot.item.itemPrice);
            }
            else
                return;
            countUIText.text = inputCount.ToString();
        }
        public void SellMinus()
        {
            var shopUI = UIManager.instance.shopUI;
            var sellSlot = UIManager.instance.shopUI.GetBuyUI().chosenSellSlot.slot;
            
            if (inputCount > 1)
            {
                inputCount--;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * sellSlot.item.itemPrice);
            }
            else
                return;
            countUIText.text = inputCount.ToString();
        }
        public void SellSlots()
        {
            var shopUI = UIManager.instance.shopUI;
            var sellSlot = UIManager.instance.shopUI.GetBuyUI().chosenSellSlot.slot;
            
            for (int i = 0; i < inputCount; i++)
            {
                if (sellSlot.count > 1)
                {
                    GameManager.instance.player.inventory.SellItem(sellSlot.item);
                }
                else if (sellSlot.count == 1)
                {
                    GameManager.instance.player.inventory.SellItem(sellSlot.item);
                    //QuickSlotItemDB.instance.CleanSlot(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                
                UIManager.instance.shopUI.GetBuyUI().DeleteSellUISlot();
                UIManager.instance.shopUI.GetBuyUI().ResetSellUISlot();
            }
            inputCount = 1;
            countUIText.text = inputCount.ToString();
            
            shopUI.hovering.boostImage.sprite = shopUI.hovering.hoverImage[1];
            DisableCountUI();
            shopUI.GetShopChat().ThankChat();
        }
    }
}
