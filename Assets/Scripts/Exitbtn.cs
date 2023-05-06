using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public ShopInventory shopinventory;
    public Sellbtn sellbtn;
    public void ClickExitbtn()
    {
        shopinventory.gameObject.SetActive(false);
        sellbtn.ButtonOnClick = false;
        Debug.Log(sellbtn.ButtonOnClick);
    }
}
