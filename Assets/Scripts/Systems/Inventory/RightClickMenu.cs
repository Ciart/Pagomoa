using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class RightClickMenu : MonoBehaviour, IPointerExitHandler
    {
        [SerializeField] private Image _instanceMenu;
        public List<Image> menu = new List<Image>();
        [SerializeField] private Image _instanceLine;
        public List<Image> lines = new List<Image>();
        [SerializeField] private Image _instanceUnderLine;
        public Image underLine;

        private UpdateItemImageSelection _refMenu;

        private void Awake()
        {
            _refMenu = _instanceMenu.GetComponent<UpdateItemImageSelection>();
            gameObject.SetActive(true);
        }


        private InventorySlot targetSlot => (InventorySlot)UIManager.instance.GetUIContainer().chosenSlot;
        
        public void PressedEquipButton() { targetSlot.clickToSlot.EquipCheck(); }
        public void PressedEquipYesButton() { targetSlot.clickToSlot.EquipItem(); }

        public void PressedEatButton()
        {
            const int mineralCount = 1; 
            targetSlot.clickToSlot.EatMineral(mineralCount);
        }

        public void PressedEatTenButton()
        {
            const int mineralCount = 10; 
            targetSlot.clickToSlot.EatMineral(mineralCount);
        }

        public void PressedEatFiftyButton()
        {
            const int mineralCount = 50;
            targetSlot.clickToSlot.EatMineral(mineralCount);
        }
        public void PressedUseButton() { targetSlot.clickToSlot.UseItem(); }
        public void PressedThrowAwayButton() { targetSlot.clickToSlot.AbandonItem(); }
        public void PressedCancelButton() { DeleteMenu(); }
        
        public void EquipmentMenu()
        {
            MakeMenu("착용하기");
            MakeMenu("버리기");
            MakeMenu("그만두기");
            MakeUnderLine();
            MenuImage();
        }
        public void MineralMenu(int itemCount)
        {
            if (itemCount >= 50)
            {
                MakeMenu("50개 먹이기");
                MakeMenu("10개 먹이기");
                MakeMenu("1개 먹이기");
                MakeMenu("버리기");
                MakeMenu("그만두기");
            }
            else if (itemCount >= 10)
            {
                MakeMenu("10개 먹이기");
                MakeMenu("1개 먹이기");
                MakeMenu("버리기");
                MakeMenu("그만두기");
            }
            else if (itemCount > 1)
            {
                MakeMenu("모두 먹이기");
                MakeMenu("1개 먹이기");
                MakeMenu("버리기");
                MakeMenu("그만두기");
            }
            else if (itemCount == 1)
            {
                MakeMenu("1개 먹이기");
                MakeMenu("버리기");
                MakeMenu("그만두기");
            }
            MakeUnderLine();
            MenuImage();
        }
        public void UseMenu()
        {
            MakeMenu("사용하기");
            MakeMenu("버리기");
            MakeMenu("그만두기");
            MakeUnderLine();
            MenuImage();
        }
        public void InherentMenu()
        {
            MakeMenu("사용하기");
            MakeMenu("그만두기");
            MakeUnderLine();
            MenuImage();
        }
        private void MakeMenu(string text)
        {
            var newLine = Instantiate(_instanceLine, this.transform);
            lines.Add(newLine);
            newLine.gameObject.SetActive(true);

            var newMenu = Instantiate(_instanceMenu, this.transform);
            menu.Add(newMenu);
            newMenu.gameObject.SetActive(true);
            newMenu.transform.GetComponentInChildren<TextMeshProUGUI>().text = text;

            if (text == "착용하기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedEquipButton);
            else if (text == "버리기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedThrowAwayButton);
            else if (text == "그만두기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedCancelButton);
            else if (text == "사용하기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedUseButton);
            else if (text == "1개 먹이기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedEatButton);
            else if (text == "10개 먹이기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedEatTenButton);
            else if (text == "50개 먹이기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedEatFiftyButton);
        }
        private void MakeUnderLine()
        {
            var underLineImage = Instantiate(_instanceUnderLine, this.transform);
            underLine = underLineImage;
            underLine.gameObject.SetActive(true);
        }
        private void MenuImage()
        {
            for (int i = 0; i < menu.Count; i++)
            {
                if (i == 0)
                    lines[i].sprite = _refMenu.GetDefaultUnderLine();
                else
                {
                    lines[i].sprite = _refMenu.GetDefaultLine();
                    lines[i].rectTransform.sizeDelta = new Vector2(50, 1);
                }
            }
        }
        public void DeleteMenu()
        {
            for (int i = 0; i < menu.Count; i++)
            {
                menu.RemoveAt(i);
            }
            for (int i = 0; i < lines.Count; i++)
            {
                lines.RemoveAt(i);    
            }
            
            menu.Clear();
            menu = new List<Image>();
            lines.Clear();
            lines = new List<Image>();
            
            underLine = null;
            for (int i = 3; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.fullyExited) DeleteMenu();
        }
    }
}
