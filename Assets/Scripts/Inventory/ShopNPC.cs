using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    public GameObject ShopUI;
    public ShopUI shopbtn;
    public GameObject EquipmentView;
    public GameObject ConsumptionView;
    public GameObject EtcView;
    public bool ActiveShopUI = false;
    private void Awake()
    {
        if (ShopUI)
            ShopUI.SetActive(ActiveShopUI);
    }
    public void SetShopUI()
    {
        if (ActiveShopUI)
        {
            ActiveShopUI = !ActiveShopUI;
            ShopUI.SetActive(ActiveShopUI);
            shopbtn.ButtonOnClick = false;
            Debug.Log(shopbtn.ButtonOnClick);
        }
        else
        {
            ActiveShopUI = !ActiveShopUI;
            ShopUI.SetActive(ActiveShopUI);
            EquipmentView.SetActive(true);
            ConsumptionView.SetActive(false);
            EtcView.SetActive(false);
        }   
    }
}
