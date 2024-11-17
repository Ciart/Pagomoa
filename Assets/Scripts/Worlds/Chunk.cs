using System;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Chunk
    {
        public const int Size = 16;
        
        // TODO: key 대신 다른 단어로 교체해야 함.
        [FormerlySerializedAs("key")] public ChunkCoords coords;

        public Brick[] bricks;

        public readonly Rect worldRect;

        public Chunk(ChunkCoords coords)
        {
            this.coords = coords;
            bricks = new Brick[Size * Size];
            worldRect = new Rect(coords.x * Size, coords.y * Size, Size, Size);

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    bricks[i + j * Size] = new Brick();
                }
            }
        }

        public Chunk(ChunkSaveData saveData)
        {
            coords = saveData.coords;
            bricks = new Brick[Size * Size];
            worldRect = new Rect(coords.x * Size, coords.y * Size, Size, Size);

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    var index = i + j * Size;
                    
                    bricks[i + j * Size] = new Brick()
                    {
                        wall = saveData.walls[index],
                    };
                }
            }
        }
        
        private bool CheckRange(float x, float y)
        {
            return 0 <= x && x < Size && 0 <= y && y < Size;
        }
    }
}