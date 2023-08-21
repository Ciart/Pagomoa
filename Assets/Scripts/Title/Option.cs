using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
   
    public void SetOption()
    {
        bool activeOption = false;

        if (gameObject.activeSelf == false)
            activeOption = !activeOption;
        
        gameObject.SetActive(activeOption);
        
    }
}
