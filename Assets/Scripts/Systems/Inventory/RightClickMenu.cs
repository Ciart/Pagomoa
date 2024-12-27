using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class RightClickMenu : MonoBehaviour
    {
        public static RightClickMenu Instance;

        [SerializeField] private GameObject _menu;
        [SerializeField] public List<GameObject> menus = new List<GameObject>();
        [SerializeField] private GameObject _line;
        [SerializeField] public List<GameObject> lines = new List<GameObject>();
        [SerializeField] public GameObject underLine;
        [SerializeField] public List<GameObject> underLines = new List<GameObject>();
        [SerializeField] public Sprite[] basicMenuImages;
        [SerializeField] public Sprite[] pressedMenuImages;
        [SerializeField] public Sprite[] hoverMenuImages;
    

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }
        public void SetUI()
        {
            if(gameObject.activeSelf == false)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }
        public void PressedEquipBtn()
        {
            UIManager.instance.bookUI.inventoryUI.choiceSlot.GetComponent<ClickToSlot>().EquipCheck();
        }
        public void PressedEquipYes()
        {
            UIManager.instance.bookUI.inventoryUI.choiceSlot.GetComponent<ClickToSlot>().EquipItem();
        }
        public void PressedEatAllBtn()
        {
            UIManager.instance.bookUI.inventoryUI.choiceSlot.GetComponent<ClickToSlot>().EatAllMineral();
        }
        public void PressedEatBtn()
        {
            UIManager.instance.bookUI.inventoryUI.choiceSlot.GetComponent<ClickToSlot>().EatMineral();
        }
        public void PressedTenEatBtn()
        {
            UIManager.instance.bookUI.inventoryUI.choiceSlot.GetComponent<ClickToSlot>().EatTenMineral();
        }
        public void PressedUseBtn()
        {
            UIManager.instance.bookUI.inventoryUI.choiceSlot.GetComponent<ClickToSlot>().UseItem();
        }
        public void PressedThrowAwayBtn()
        {
            UIManager.instance.bookUI.inventoryUI.choiceSlot.GetComponent<ClickToSlot>().AbandonItem();
        }
        public void PressedCancleBtn()
        {
            SetUI();
            DeleteMenu();
        }
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
            if (itemCount >= 10)
            {
                MakeMenu("모두 먹이기");
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
            GameObject newLine = Instantiate(_line, this.transform);
            lines.Add(newLine);
            newLine.SetActive(true);

            GameObject newMenu = Instantiate(_menu, this.transform);
            menus.Add(newMenu);
            newMenu.SetActive(true);
            newMenu.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

            if (text == "착용하기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedEquipBtn);
            else if (text == "버리기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedThrowAwayBtn);
            else if (text == "그만두기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedCancleBtn);
            else if (text == "사용하기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedUseBtn);
            else if (text == "10개 먹이기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedTenEatBtn);
            else if (text == "모두 먹이기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedEatAllBtn);
            else if (text == "1개 먹이기")
                newMenu.GetComponent<Button>().onClick.AddListener(PressedEatBtn);
        }
        private void MakeUnderLine()
        {
            GameObject UnderLine = Instantiate(underLine, this.transform);
            underLines.Add(UnderLine);
            UnderLine.SetActive(true);
        }
        private void MenuImage()
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (i == 0)
                    lines[i].GetComponent<Image>().sprite = basicMenuImages[1];
                else
                {
                    lines[i].GetComponent<Image>().sprite = basicMenuImages[0];
                    lines[i].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 1);
                }
            }
        }
        public void DeleteMenu()
        {
            menus.Clear();
            lines.Clear();
            underLines.Clear();
            for (int i = 3; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
        }
    }
}
