using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    public void SetUI()
    {
        Debug.Log(this.gameObject.activeSelf);
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
    public void PressedEquipBtn()
    {
        EtcInventory.Instance.choiceSlot.GetComponent<ClickSlot>().EquipCheck();
    }
    public void PressedEquipYes()
    {
        EtcInventory.Instance.choiceSlot.GetComponent<ClickSlot>().EquipItem();
    }
    public void PressedEatBtn()
    {
        EtcInventory.Instance.choiceSlot.GetComponent<ClickSlot>().EatMineral();
    }
    public void PressedTenEatBtn()
    {
        EtcInventory.Instance.choiceSlot.GetComponent<ClickSlot>().EatTenMineral();
    }
    public void PressedUseBtn()
    {
        EtcInventory.Instance.choiceSlot.GetComponent<ClickSlot>().UseItem();
    }
    public void PressedThrowAwayBtn()
    {
        EtcInventory.Instance.choiceSlot.GetComponent<ClickSlot>().AbandonItem();
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
    }
    public void MineralMenu()
    {
        MakeMenu("10개 먹이기");
        MakeMenu("1개 먹이기");
        MakeMenu("버리기");
        MakeMenu("그만두기");
    }
    public void UseMenu()
    {
        MakeMenu("사용하기");
        MakeMenu("버리기");
        MakeMenu("그만두기");
    }
    public void InherentMenu()
    {
        MakeMenu("사용하기");
        MakeMenu("그만두기");
    }
    private void MakeMenu(string text)
    {
        GameObject newMenu = Instantiate(_menu, this.transform);
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

        else if (text == "1개 먹이기")
            newMenu.GetComponent<Button>().onClick.AddListener(PressedEatBtn);
    }
    public void DeleteMenu()
    {
        for(int i = 1; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
