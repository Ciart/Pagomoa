using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sellbtn : MonoBehaviour
{
    public bool ButtonOnClick = false;

    public void Sellbtnclick()
    {
        ButtonOnClick = true;
        Debug.Log(ButtonOnClick);
    }
}
