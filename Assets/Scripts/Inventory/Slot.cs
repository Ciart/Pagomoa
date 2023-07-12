using Maps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item mineralItem;
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    public int id;
    public void SetItem(Item data, int count)
    {
        mineralItem = data;
        image.sprite = data.itemImage;
        text.text = count.ToString();
    }
    public void RemoveItem()
    {
        mineralItem = null;
        image.sprite = null;
        text.text = "";
    }
}
