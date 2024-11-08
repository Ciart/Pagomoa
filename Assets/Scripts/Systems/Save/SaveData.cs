using System.Collections.Generic;
using Ciart.Pagomoa.Worlds;
using MemoryPack;

namespace Ciart.Pagomoa.Systems.Save
{
    [MemoryPackable]
    public partial record SaveData
    {
        public WorldSaveData worldSaveData;
    }

    [MemoryPackable]
    public partial record WorldSaveData
    {
        public LevelSaveData[] levels;
    }

    [MemoryPackable]
    public partial record LevelSaveData
    {
        public string id;
        
        public LevelType type;
        
        public int top;

        public int bottom;

        public int left;

        public int right;
        
        // public List<EntityData> entityDataList;
        
        public ChunkSaveData[] chunks;
    }

    [MemoryPackable]
    public partial record ChunkSaveData
    {
        public ChunkCoords coords;

        public string[] walls;

        public string[] grounds;

        public string[] minerals;

        public bool[] isRocks;
    }
}