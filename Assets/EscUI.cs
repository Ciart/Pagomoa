using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscUI : MonoBehaviour
{
    [SerializeField] public GameObject optionUI;
    
    public void SetOption()
    {
        bool activeOption = false;
        if(optionUI.activeSelf == false)
            activeOption = !activeOption;
        optionUI.SetActive(activeOption);
        transform.gameObject.SetActive(false);
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
