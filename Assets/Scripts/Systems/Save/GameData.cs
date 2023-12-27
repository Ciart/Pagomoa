using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Players;
using Inventory;
using Unity.VisualScripting;
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
    public QuickSlotData quickSlotData;
    public PlayerCurrentStatusData playerStatusData;
    public LogGeneralData gameLogData;
}

[System.Serializable]
public class IntroData
{
    public bool isFirstStart;
}

[System.Serializable]
public class LogGeneralData
{
    /*public List<int> values = new List<int>();
    
    
    
    public void SetGeneralObjectCount(List<GameLogger.LoggingGeneral> logs)
    {
        values.Clear();
        
        foreach (GameLogger.LoggingGeneral obj in logs)
        {
            values.Add(GameLogger.Instance.GetObjectCount(obj));
        }
    }*/
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
    public int top;

    public int bottom;

    public int left;

    public int right;
    
    public List<WorldEntityData> entityDataList;

    public DicList<Vector2Int, Chunk> _chunks;

    public void SetWorldDataFromWorld(World world)
    {
        top = world.top;
        bottom = world.bottom;
        left = world.left;
        right = world.right;
        entityDataList = world.entityDataList;
        _chunks = ListDictionaryConverter.ToList(world.GetAllChunks(), true);
    }
}
[System.Serializable]
public class ItemData
{
    public List<InventoryItem> items = new List<InventoryItem>(new InventoryItem[30]);
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
[System.Serializable]
public class QuickSlotData
{
    public List<InventoryItem> items;
    public int selectedSlotID = -1;
    public void SetQuickSlotDataFromQuickSlotDB(QuickSlotItemDB quickSlotItemDB)
    {
        items = quickSlotItemDB.quickSlotItems;
        if(quickSlotItemDB.selectedSlot)
            selectedSlotID = quickSlotItemDB.selectedSlot.id;
    }
}
[System.Serializable]
public class PlayerCurrentStatusData
{
    public float currentOxygen;
    public float currentHungry;

    public void SetCurrentStatusData(PlayerStatus playerStatus)
    {
        currentOxygen = playerStatus.oxygen;
        currentHungry = playerStatus.hungry;
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
