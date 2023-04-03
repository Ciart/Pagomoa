using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image itemIcon;
    public Text text;
    public int index;
    public void UpdateSlot(Sprite image, int count)
    {
        itemIcon.sprite = image;
        itemIcon.gameObject.SetActive(true);
        text.text = count.ToString();
    }
    public void UpdateCount(int count)
    {
        text.text = (int.Parse(text.text) + count).ToString();
    }
}

