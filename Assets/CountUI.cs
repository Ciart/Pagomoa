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
            countUpButton.onClick.RemoveAllListeners();
            countDownButton.onClick.RemoveAllListeners();
            
            if (type == SlotType.Buy || type == SlotType.BuyArtifact)
            {
                countUpButton.onClick.AddListener(BuyPlus);
                countDownButton.onClick.AddListener(BuyMinus);
                buyYesButton.gameObject.SetActive(true);
                buyNoButton.gameObject.SetActive(true);
            }
            else if (type == SlotType.Sell)
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
            else if (_chosenSlotType == SlotType.Sell)
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
            var buySlot = shopUI.chosenSlot;
            
            if (buySlot.GetSlotItem().type == ItemType.Use)
            {
                inputCount++;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * buySlot.GetSlotItem().price);
            }
            else if (buySlot.GetSlotItem().type == ItemType.Equipment || buySlot.GetSlotItem().type == ItemType.Inherent)
            {
                if (inputCount < buySlot.GetSlotItemCount())
                    inputCount++;
            }
            
            countUIText.text = inputCount.ToString();
        }
        
        public void BuyMinus()
        {
            var shopUI = UIManager.instance.shopUI;
            var buySlot = shopUI.chosenSlot;
            
            if (inputCount > 1)
            {
                inputCount--;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * buySlot.GetSlotItem().price);
            }
            
            countUIText.text = inputCount.ToString();
        }
        public void BuySlots()
        {
            var inventory = GameManager.instance.player.inventory;
            var shopUI = UIManager.instance.shopUI;
            var buySlot = shopUI.chosenSlot;
            
            if (buySlot.GetSlotItem().type == ItemType.Use)
            {
                if (inventory.Gold >= buySlot.GetSlotItem().price * inputCount && inputCount > 0)
                {
                    inventory.Add(buySlot.GetSlotItem(), inputCount);
                    shopUI.GetShopItems().Remove(buySlot.GetSlotItem());
                    shopUI.hovering.boostImage.sprite = shopUI.hovering.hoverImage[1];
                    DisableCountUI();
                }
                else
                    return;
                
                shopUI.GetShopChat().ThankChat();
            }
            else if (buySlot.GetSlotItem().type == ItemType.Equipment || buySlot.GetSlotItem().type == ItemType.Inherent)
            {
                if (inventory.Gold >= buySlot.GetSlotItem().price && buySlot.GetSlotItemCount() == inputCount)
                {
                    inventory.Add(buySlot.GetSlotItem(), 0);
                    shopUI.GetShopItems().Remove(buySlot.GetSlotItem());
                    shopUI.hovering.boostImage.sprite = shopUI.hovering.hoverImage[1];
                    DisableCountUI();
                    
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
            var sellSlot = UIManager.instance.shopUI.chosenSlot;
            
            if (inputCount < sellSlot.GetSlotItemCount())
            {
                inputCount++;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * sellSlot.GetSlotItem().price);
            }
            else
                return;
            countUIText.text = inputCount.ToString();
        }
        public void SellMinus()
        {
            var shopUI = UIManager.instance.shopUI;
            var sellSlot = UIManager.instance.shopUI.chosenSlot;
            
            if (inputCount > 1)
            {
                inputCount--;
                shopUI.GetShopChat().TotalPriceToChat(inputCount * sellSlot.GetSlotItem().price);
            }
            else
                return;
            countUIText.text = inputCount.ToString();
        }
        public void SellSlots()
        {
            var inventory = GameManager.instance.player.inventory;
            var shopUI = UIManager.instance.shopUI;
            var sellSlot = UIManager.instance.shopUI.chosenSlot;
            
            for (int i = 0; i < inputCount; i++)
            {
                if (sellSlot.GetSlotItemCount() > 1)
                {
                    inventory.SellItem(sellSlot.GetSlotItem());
                }
                else if (sellSlot.GetSlotItemCount() == 1)
                {
                    inventory.SellItem(sellSlot.GetSlotItem());
                    //QuickSlotItemDB.instance.CleanSlot(Sell.Instance.choiceSlot.inventoryItem.item);
                }
                
                UIManager.instance.shopUI.GetSellUI().DeleteSellUISlot();
                UIManager.instance.shopUI.GetSellUI().UpdateSellUISlot();
            }
            inputCount = 1;
            countUIText.text = inputCount.ToString();
            
            shopUI.hovering.boostImage.sprite = shopUI.hovering.hoverImage[1];
            DisableCountUI();
            shopUI.GetShopChat().ThankChat();
        }
    }
}
