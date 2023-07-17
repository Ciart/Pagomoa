using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sell : EtcInventory
{
    [SerializeField] GameObject sellUI;
    private bool click = true;
    public void SetUI()
    {
        sellUI.SetActive(click);
        click = !click;
    }
}
