using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour
{
    static public DragItem Instance;

    void Start()
    {
        Instance = this;
    }
    public void DragSetImage(Sprite image)
    {
        gameObject.GetComponent<Image>().sprite = image;
        SetColor(230);
    }    
    public void SetColor(float a)
    {
        Color color = gameObject.GetComponent<Image>().color;
        color.a = a;
        gameObject.GetComponent<Image>().color = color;
    }
}
