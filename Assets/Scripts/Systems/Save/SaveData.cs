using System.Collections.Generic;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Worlds;
using MemoryPack;

namespace Ciart.Pagomoa.Systems.Save
{
    [MemoryPackable]
    public partial class SaveData
    {
        public int version = 0;

        public WorldSaveData world = new WorldSaveData();
        
        public PlayerSaveData player = new PlayerSaveData();

        public List<QuestSaveData> quests = new List<QuestSaveData>();
    }

    /// <summary>
    /// 스탯과는 관련 없는 플레이어 고유의 데이터입니다.
    /// </summary>
    [MemoryPackable]
    public partial class PlayerSaveData
    {
        public InventorySaveData inventory;
    }

    [MemoryPackable]
    public partial class InventorySaveData
    {
        public InventorySlotSaveData[] quickSlots;

        public InventorySlotSaveData[] artifactSlots;

        public InventorySlotSaveData[] inventorySlots;
    }

    [MemoryPackable]
    public partial class InventorySlotSaveData
    {
        public string id;
        public int count;
    }

    [MemoryPackable]
    public partial class WorldSaveData
    {
        public LevelSaveData[] levels;
    }

    [MemoryPackable]
    public partial class LevelSaveData
    {
        public string id;
        
        public LevelType type;
        
        public int top;

        public int bottom;

        public int left;

        public int right;
        
        public EntitySaveData[] entities;
        
        public ChunkSaveData[] chunks;
    }
    
    [MemoryPackable]
    public partial class EntitySaveData
    {
        public string id;

        public int x;
        
        public int y;
    }

    [MemoryPackable]
    public partial class ChunkSaveData
    {
        public ChunkCoords coords;

        public string[] walls;

        public string[] grounds;

        public string[] minerals;

        public bool[] isRocks;
    }

    [MemoryPackable]
    public partial class QuestSaveData
    {
        public string id;

        public QuestState state;

        public QuestConditionSaveData[] conditions;
    }

    [MemoryPackable]
    public partial class QuestConditionSaveData
    {
        public int compareValue;
    }
}
