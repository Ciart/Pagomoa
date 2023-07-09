using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject EquipmentView;
    public GameObject ConsumptionView;
    public GameObject EtcView;
    public bool ButtonOnClick = false;
    //public ShopInventory sellUI;
    public BuyUI buyUI;
    public ShopNPC shopnpc;

    public void ClickEquipmentButton()
    {
        EquipmentView.SetActive(true);
        ConsumptionView.SetActive(false);
        EtcView.SetActive(false);
    }
    public void ClickConsumptionButton()
    {
        ConsumptionView.SetActive(true);
        EquipmentView.SetActive(false);
        EtcView.SetActive(false);
    }
    public void ClickEtcButton()
    {
        EtcView.SetActive(true);
        EquipmentView.SetActive(false);
        ConsumptionView.SetActive(false);
    }
    public void ShopUIbtnclick()
    {
        ButtonOnClick = true;
        Debug.Log(ButtonOnClick);
    }
    public void ClickSellExitbtn()
    {
        //sellUI.gameObject.SetActive(false);
        ButtonOnClick = false;
        shopnpc.ActiveShopUI = false;
        Debug.Log(ButtonOnClick);
    }
    public void ClickBuyExitbtn()
    {
        buyUI.gameObject.SetActive(false);
        ButtonOnClick = false;
        shopnpc.ActiveShopUI = false;
        Debug.Log(ButtonOnClick);
    }
}
