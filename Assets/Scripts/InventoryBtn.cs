using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBtn : MonoBehaviour
{
    public GameObject EquipmentView;
    public GameObject ConsumptionView;
    public GameObject EtcView;

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
}
