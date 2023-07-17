using Maps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Slot : MonoBehaviour
{
    public Item item;
    [SerializeField] public Image image;
    [SerializeField] public Text text;

    public void ClickSlot()
    {
        EtcInventory.Instance.DeleteSlot();
        InventoryDB.Instance.Remove(item);
        EtcInventory.Instance.UpdateSlot();
    }
    public void SetUI(Sprite s, string m)
    {
        image.sprite = s;
        text.text = m;
    }
}