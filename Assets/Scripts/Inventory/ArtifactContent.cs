using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactContent : MonoBehaviour
{
    [SerializeField] private ArtifactSlotDB artifactSlotDB;
    [SerializeField] private Sprite image;
    [SerializeField] private List<Slot> slotDatas = new List<Slot>();

    private static ArtifactContent instance;
    public static ArtifactContent Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(ArtifactContent)) as ArtifactContent;
            }
            return instance;
        }
    }
    public void ResetSlot() 
    {
        int i = 0;
        for (; i < artifactSlotDB.Artifact.Count && i < slotDatas.Count; i++)
        {
            slotDatas[i].inventoryItem = artifactSlotDB.Artifact[i];
        }
        for (; i < slotDatas.Count; i++)
        {
            slotDatas[i].inventoryItem = null;
        }
        UpdateSlot();
    }
    public void UpdateSlot()
    {
        DeleteSlot();
        for (int i = 0; i < artifactSlotDB.Artifact.Count; i++)
        {
            slotDatas[i].SetUI(artifactSlotDB.Artifact[i].item.itemImage);
        }
    }
    public void DeleteSlot()
    {
        if (artifactSlotDB.Artifact.Count >= 0)
        {
            for (int i = 0; i < slotDatas.Count; i++)
                slotDatas[i].SetUI(image);
        }
    }
}
