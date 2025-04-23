using System;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Chunk
    {
        public const int Size = 16;
        
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
                        wallId = saveData.walls[index],
                        groundId = saveData.grounds[index],
                        mineralId = saveData.minerals[index],
                        isRock = saveData.isRocks[index]
                    };
                }
            }
        }
        
        public ChunkSaveData CreateSaveData()
        {
            const int arraySize = Size * Size;
            
            var walls = new string[arraySize];
            var grounds = new string[arraySize];
            var minerals = new string[arraySize];
            var isRooks = new bool[arraySize];

            for (var i = 0; i < arraySize; i++)
            {
                var brick = bricks[i];
                
                walls[i] = brick.wallId;
                grounds[i] = brick.groundId;
                minerals[i] = brick.mineralId;
                isRooks[i] = brick.isRock;
            }
            
            return new ChunkSaveData()
            {
                coords = coords,
                walls = walls,
                grounds = grounds,
                minerals = minerals,
                isRocks = isRooks
            };
        }
        
        private bool CheckRange(float x, float y)
        {
            return 0 <= x && x < Size && 0 <= y && y < Size;
        }

        public IEnumerable<(int x, int y, int index)> GetBrickPositionsAndIndices()
        {
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    yield return (x, y, x + y * Size);
                }
            }
        }
    }
}
