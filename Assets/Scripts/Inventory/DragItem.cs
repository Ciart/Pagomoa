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
        Debug.Log("호출되니");
        HoverEvent.Instance.hoverRenderer.SetActive(false);
        //SetColor(1);
    }    
    public void SetColor(float a)
    {
        Color color = gameObject.GetComponent<Image>().color;
        color.a = a;
        gameObject.GetComponent<Image>().color = color;
    }
}
