using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Worlds;

[System.Serializable]
public class GameData
{
    public WorldData worldData;
    public PositionData posData;
    public IntroData introData;
    public ItemData itemData;
    public OptionData optionData;
    public ArtifactData artifactData;
}

[System.Serializable]
public class IntroData
{
    public bool isFirstStart;
} 

[System.Serializable]
public class PositionData
{
    public DicList<string, Vector3> positionData;
    public void SetPositionDataFromDictionary(Dictionary<string, Vector3> posData)
    {
        positionData = ListDictionaryConverter.ToList(posData);
    }
}
[System.Serializable]
public class WorldData
{
    public int chunkSize;

    public int top;

    public int bottom;

    public int left;

    public int right;

    public int groundHeight = 0;

    public DicList<Vector2Int, Chunk> _chunks;

    public void SetWorldDataFromWorld(World world)
    {
        chunkSize = world.chunkSize;
        top = world.top;
        bottom = world.bottom;
        left = world.left;
        right = world.right;
        groundHeight = world.groundHeight;
        _chunks = ListDictionaryConverter.ToList(world.GetAllChunks(), true);
    }
}
[System.Serializable]
public class ItemData
{
    public List<InventoryItem> items;
    public int gold;
    public void SetItemDataFromInventoryDB(InventoryDB inventoryDB)
    {
        items = inventoryDB.items;
        gold = inventoryDB.Gold;
    }
}
[System.Serializable]
public class OptionData
{
    public int scale = 1;
    public float audioValue = 0;
    public void SetOptionDataFromOptionDB(OptionDB optionDB)
    {
        if (!optionDB) return;
        scale = optionDB.scale;
        audioValue = optionDB.audioValue;
    }
}
[System.Serializable]
public class ArtifactData
{
    public List<InventoryItem> artifacts;

    public void SetArtifactDataFromArtifactSlotDB(ArtifactSlotDB artifactSlotDB)
    {
        artifacts = artifactSlotDB.Artifact;
    }
}


// �ڷᱸ��

[System.Serializable]
public class DataDictionary<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

[System.Serializable]
public class DicList<TKey, TValue>
{
    public List<DataDictionary<TKey, TValue>> data;
}


public static class ListDictionaryConverter
{


    public static DicList<TKey, TValue> ToList<TKey, TValue>(Dictionary<TKey, TValue> jsonDicData, bool pretty = false)
    {
        List<DataDictionary<TKey, TValue>> dataList = new List<DataDictionary<TKey, TValue>>();
        DataDictionary<TKey, TValue> dictionaryData;
        foreach (TKey key in jsonDicData.Keys)
        {
            dictionaryData = new DataDictionary<TKey, TValue>();
            dictionaryData.Key = key;
            dictionaryData.Value = jsonDicData[key];
            dataList.Add(dictionaryData);
        }
        DicList<TKey, TValue> arrayJson = new DicList<TKey, TValue>();
        arrayJson.data = dataList;

        return arrayJson;
    }
    //JsonUtility.FromJson<JsonDataArray<TKey, TValue>>(jsonData);
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(DicList<TKey, TValue> dataList)
    {
        Dictionary<TKey, TValue> returnDictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < dataList.data.Count; i++)
        {
            DataDictionary<TKey, TValue> dictionaryData = dataList.data[i];
            returnDictionary[dictionaryData.Key] = dictionaryData.Value;
        }

        return returnDictionary;
    }
}
