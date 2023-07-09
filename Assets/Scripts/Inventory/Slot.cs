using Maps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Mineral mineral;
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    public int id;
    public void SetItem(Mineral data, int count)
    {
        mineral = data;
        image.sprite = data.sprite;
        text.text = count.ToString();
    }
    public void RemoveItem()
    {
        mineral = null;
        image.sprite = null;
        text.text = "";
    }
}
