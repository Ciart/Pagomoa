using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.UI.Title;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Save
{
    [System.Serializable]
    public class GameData
    {
        public WorldData worldData;
        public PositionData posData;
        public IntroData introData;
        public ItemData itemData;
        // public OptionData optionData;
        public ArtifactData artifactData;
        public QuickSlotData quickSlotData;
        public MineralData mineralData;
        public PlayerCurrentStatusData playerStatusData;
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
        public DicList<string, LevelData> levelData;
    }

    [System.Serializable]
    public class LevelData
    {
        public int top;

        public int bottom;

        public int left;

        public int right;
    
        public List<EntityData> entityDataList;

        public DicList<ChunkCoords, Chunk> _chunks;

        public void SetLevelDataFromLevel(Level level)
        {
            top = level.top;
            bottom = level.bottom;
            left = level.left;
            right = level.right;
            entityDataList = level.entityDataList;
            _chunks = ListDictionaryConverter.ToList(level.GetAllChunks(), true);
        }
    }

    [System.Serializable]
    public class ItemData
    {
        /*const int MaxItems = 30;
        public Slot[] items = new Slot[MaxItems];
        public int gold;
        public void SetItemDataFromInventoryDB(Inventory.Inventory inventory)
        {
            var index = 0;
            foreach (var item in inventory.GetSlots(SlotType.Inventory))
            {
                items[index] = inventory.inventorySlots[index];
                index++;
            }
            
            gold = inventory.gold;
        }*/
    }

    // [System.Serializable]
    // public class OptionData
    // {
    //     public int scale = 1;
    //     public float audioValue = 0;
    //     public void SetOptionDataFromOptionDB(OptionDB optionDB)
    //     {
    //         if (!optionDB) return;
    //         scale = optionDB.scale;
    //         audioValue = optionDB.audioValue;
    //     }
    // }

    [System.Serializable]
    public class ArtifactData
    {
        public List<InventorySlotUI> artifacts;

        // public void SetArtifactDataFromArtifactSlotDB(ArtifactSlotDB artifactSlotDB)
        // {
        //     artifacts = artifactSlotDB.Artifact;
        // }
    }

    [System.Serializable]
    public class QuickSlotData
    {
        // public List<InventoryItem> items;
        // public int selectedSlotID = -1;
        // public void SetQuickSlotDataFromQuickSlotDB(QuickSlotItemDB quickSlotItemDB)
        // {
        //     items = quickSlotItemDB.quickSlotItems;
        //     if(quickSlotItemDB.selectedSlot)
        //         selectedSlotID = quickSlotItemDB.selectedSlot.id;
        // }
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

    [System.Serializable]
    public class MineralData
    {
        /*public int eatenMineralCount;

        public void SetEatenMineralData(Inventory.Inventory stoneCount)
        {
            eatenMineralCount = stoneCount.stoneCount;
        }*/
    }

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
}