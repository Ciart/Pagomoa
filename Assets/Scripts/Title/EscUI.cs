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
        gameObject.SetActive(false);
       
        OptionDB.instance.audioValue = Option.instance.audio.audioSlider.value;
        OptionDB.instance.scale = (int)Option.instance.canvas.scaleFactor;
        Debug.Log((int)Option.instance.canvas.scaleFactor);
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
