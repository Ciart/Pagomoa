using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image itemIcon;
    public Text text;
    public int index;
    public Sellbtn sellbutton;
    public Inventory inventory;
    public List<Inventory> inventorydatabases = new List<Inventory>();
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
    public void Choice()
    {
        if (sellbutton.ButtonOnClick)
        {
            if(int.Parse(text.text) > 1)
            {
                string abc = (int.Parse(text.text) - 1).ToString();
                for (int n = 0; n < inventorydatabases.Count; n++)
                {
                    for (int i = 0; i < inventory.Minerals.Count; i++)
                    {
                        if (inventory.Minerals[i].sprite == itemIcon.sprite)
                        {
                            for (int j = 0; j < inventorydatabases.Count; j++)
                            {
                                inventorydatabases[j].slots.transform.GetChild(i + 1).GetComponent<Slot>().text.text = abc;
                            }
                        }
                    }
                }
            }
            else if(int.Parse(text.text) == 1)
            {
                for (int n = 0; n < inventorydatabases.Count; n++)
                {
                    for (int i = 0; i < inventory.Minerals.Count; i++)
                    {
                        if (inventory.Minerals[i].sprite == itemIcon.sprite)
                        {
                            Debug.Log("´­·È³Ä?");
                            for(int j = 0; j < inventorydatabases.Count; j++)
                            {
                                Destroy(inventorydatabases[j].slots.transform.GetChild(i + 1).gameObject);
                                inventorydatabases[j].Minerals.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
    }
}

