﻿using System;
using System.Linq;
using Ciart.Pagomoa.UI;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    [Serializable]
    public class BookTabItem
    {
        public TabSpriteButton tabButton;
        public GameObject content;
    }
    
    public class BookUI : MonoBehaviour
    {
        public BookTabItem[] tabItems;
        
        [SerializeField] private InventoryUI _inventoryUI;
        public InventoryUI GetInventoryUI() => _inventoryUI;
        
        [SerializeField] private RightClickMenu _rightClickMenu;
        public RightClickMenu GetRightClickMenu() => _rightClickMenu;
        [SerializeField] private  HoverItemInfo _hoverItemInfo;
        public HoverItemInfo GetHoverItemInfo() => _hoverItemInfo;

        public void ActiveBook()
        {
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);
        }

        public void DeActiveBook()
        {
            if (!gameObject.activeSelf) return;
            gameObject.SetActive(false);
            Game.Instance.Time.ResumeTime();
        }
        
        private void OnClickTab(int index)
        {
            for (var i = 0; i < tabItems.Length; i++)
            {
                tabItems[i].content.SetActive(i == index);
                tabItems[i].tabButton.isSelected = i == index;
            }
        }
        
        private void Awake()
        {
            foreach (var (i, tabItem) in tabItems.Select((value, index) => (index, value)))
            {
                tabItem.tabButton.onClick.AddListener(() => OnClickTab(i));
            }
        }
    }
}
