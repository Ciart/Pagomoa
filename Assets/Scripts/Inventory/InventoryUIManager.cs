using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] GameObject UI;
   

    public void SetUI()
    {
        bool click = false;
        if(UI.activeSelf == false)
            click = !click;
        UI.SetActive(click);
    }
}
