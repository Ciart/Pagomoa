using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SellNPC : MonoBehaviour
{
    public GameObject SellWindowUI;
    public Sellbtn sellbtn;
    public bool ActiveSellWindow = false;
    private void Awake()
    {
        if (SellWindowUI)
            SellWindowUI.SetActive(ActiveSellWindow);
    }
    public void SetSellWindow()
    {
        if (ActiveSellWindow)
        {
            ActiveSellWindow = !ActiveSellWindow;
            SellWindowUI.SetActive(ActiveSellWindow);
            sellbtn.ButtonOnClick = false;
            Debug.Log(sellbtn.ButtonOnClick);
           
        }
        else
        {
            ActiveSellWindow = !ActiveSellWindow;
            SellWindowUI.SetActive(ActiveSellWindow);
        }
    }
   
}
