using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndQuestion : MonoBehaviour
{
    public void SetUI()
    {
        bool activeUI = false;
        if (gameObject.activeSelf == true)
            activeUI = false;
        else
            activeUI = true;
        gameObject.SetActive(activeUI);
    }
}
