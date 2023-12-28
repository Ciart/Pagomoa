using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public List<InventoryItem> AchieveMinerals = new List<InventoryItem>();

    private static Achievements instance;
    public static Achievements Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(Achievements)) as Achievements;
            }
            return instance;
        }
    }
}
