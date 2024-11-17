using System.Collections.Generic;
using Ciart.Pagomoa.Worlds;
using MemoryPack;

namespace Ciart.Pagomoa.Systems.Save
{
    [MemoryPackable]
    public partial class SaveData
    {
        public WorldSaveData world;
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
}