using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ArtifactSlotDB : MonoBehaviour
{
    public List<InventoryItem> Artifact = new List<InventoryItem>();

    private static ArtifactSlotDB instance;
    public static ArtifactSlotDB Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(ArtifactSlotDB)) as ArtifactSlotDB;
            }
            return instance;
        }
    }

    public void Remove(Itembefore data)
    {
        var inventoryItem = Artifact.Find(inventoryItem => inventoryItem.item == data);
        if (inventoryItem != null)
        {
            Artifact.Remove(inventoryItem);
        }
    }
}
