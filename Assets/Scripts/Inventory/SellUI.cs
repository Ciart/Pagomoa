using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellUI : MonoBehaviour
{
    public GameObject ConsumptionView;
    public GameObject EtcView;
    public bool ButtonOnClick = false;
    //public ShopInventory sellUI;
    public SellNPC sellnpc;

    public void SellClickConsumptionButton()
    {
        ConsumptionView.SetActive(true);
        EtcView.SetActive(false);
    }
    public void SellClickEtcButton()
    {
        EtcView.SetActive(true);
        ConsumptionView.SetActive(false);
    }
    public void SellShopUIbtnclick()
    {
        ButtonOnClick = true;
    }
    public void SellClickSellExitbtn()
    {
        //sellUI.gameObject.SetActive(false);
        ButtonOnClick = false;
        sellnpc.ActiveShopUI = false;
    }
}
