using System;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.UI;
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
        
        // [SerializeField] public GameObject ItemHoverObject;
        
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
            
            OnClickTab(0);
        }
        
        [Obsolete("프로토타입에서 사용하는 함수 입니다.")]
        public void SetUI()
        {
            bool click = false;
            if (gameObject.activeSelf == false)
                click = !click;
            gameObject.SetActive(click);
        }
    }
}
