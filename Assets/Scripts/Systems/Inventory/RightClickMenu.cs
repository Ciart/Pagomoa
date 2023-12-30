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
        MakeMenu("�����ϱ�");
        MakeMenu("������");
        MakeMenu("�׸��α�");
    }
    public void MineralMenu()
    {
        MakeMenu("10�� ���̱�");
        MakeMenu("1�� ���̱�");
        MakeMenu("������");
        MakeMenu("�׸��α�");
    }
    public void UseMenu()
    {
        MakeMenu("����ϱ�");
        MakeMenu("������");
        MakeMenu("�׸��α�");
    }
    public void InherentMenu()
    {
        MakeMenu("����ϱ�");
        MakeMenu("�׸��α�");
    }
    private void MakeMenu(string text)
    {
        GameObject newMenu = Instantiate(_menu, this.transform);
        newMenu.SetActive(true);
        newMenu.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        if (text == "�����ϱ�")
            newMenu.GetComponent<Button>().onClick.AddListener(PressedEquipBtn);

        else if (text == "������")
            newMenu.GetComponent<Button>().onClick.AddListener(PressedThrowAwayBtn);

        else if (text == "�׸��α�")
            newMenu.GetComponent<Button>().onClick.AddListener(PressedCancleBtn);

        else if (text == "����ϱ�")
            newMenu.GetComponent<Button>().onClick.AddListener(PressedUseBtn);

        else if (text == "10�� ���̱�")
            newMenu.GetComponent<Button>().onClick.AddListener(PressedTenEatBtn);

        else if (text == "1�� ���̱�")
            newMenu.GetComponent<Button>().onClick.AddListener(PressedEatBtn);
    }
    public void DeleteMenu()
    {
        for(int i = 1; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
