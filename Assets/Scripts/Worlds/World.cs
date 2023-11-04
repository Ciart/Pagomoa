using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Worlds
{
    [Serializable]
    public class Chunk
    {
        // TODO: key 대신 다른 단어로 교체해야 함.
        public Vector2Int key;

        public Brick[] bricks;

        public List<WorldEntityData> entityDataList;

        public readonly Rect worldRect;

        public Chunk(Vector2Int key)
        {
            this.key = key;
            bricks = new Brick[World.ChunkSize * World.ChunkSize];
            entityDataList = new List<WorldEntityData>();
            worldRect = new Rect(key.x * World.ChunkSize, key.y * World.ChunkSize, World.ChunkSize, World.ChunkSize);

            for (var i = 0; i < World.ChunkSize; i++)
            {
                for (var j = 0; j < World.ChunkSize; j++)
                {
                    bricks[i + j * World.ChunkSize] = new Brick();
                }
            }
        }
        
        public void AddEntity(float x, float y, Entity entity, EntityStatus status = null)
        {
            entityDataList.Add(new WorldEntityData(x, y, entity, status));
        }

        public void AddEntity(WorldEntityData entityData)
        {
            entityDataList.Add(entityData);
        }

        private bool CheckRange(float x, float y)
        {
            return 0 <= x && x < World.ChunkSize && 0 <= y && y < World.ChunkSize;
        }
    }

    public class World
    {
        public const int ChunkSize = 16;

        public readonly int top;

        public readonly int bottom;

        public readonly int left;

        public readonly int right;

        public readonly int groundHeight = 0;
        private readonly Dictionary<Vector2Int, Chunk> _chunks;

        public World(int top, int bottom, int left, int right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
            
            _chunks = new Dictionary<Vector2Int, Chunk>();

            for (var keyX = -this.left; keyX < this.right; keyX++)
            {
                for (var keyY = -this.bottom; keyY < this.top; keyY++)
                {
                    var key = new Vector2Int(keyX, keyY);
                    _chunks[key] = new Chunk(key);
                }
            }
        }

        public World(WorldData worldData)
        {
            this.top = worldData.top;
            this.bottom = worldData.bottom;
            this.left = worldData.left;
            this.right = worldData.right;

            _chunks = ListDictionaryConverter.ToDictionary(worldData._chunks);
        }

        public Chunk GetChunk(Vector2Int key)
        {
            return _chunks.TryGetValue(key, out var chunk) ? chunk : null;
        }

        public Chunk GetChunk(int x, int y)
        {
            var key = new Vector2Int(Mathf.FloorToInt((float)x / ChunkSize), Mathf.FloorToInt((float)y / ChunkSize));

            return GetChunk(key);
        }

        public Dictionary<Vector2Int, Chunk> GetAllChunks()
        {
            return _chunks;
        }

        public Brick GetBrick(int x, int y, out Chunk chunk)
        {
            chunk = GetChunk(x, y);

            if (chunk is null)
            {
                return null;
            }

            var brinkX = x < 0 ? ChunkSize - 1 + (x + 1) % ChunkSize : x % ChunkSize;
            var brinkY = y < 0 ? ChunkSize - 1 + (y + 1) % ChunkSize : y % ChunkSize;

            return chunk.bricks[brinkX + brinkY * ChunkSize];
        }

        public void AddEntity(float x, float y, Entity entity, EntityStatus status = null)
        {
            var chunk = GetChunk(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        
            if (chunk is null)
            {
                return;
            }
            
            chunk.AddEntity(x - chunk.key.x * ChunkSize, y - chunk.key.y * ChunkSize, entity, status);
        }
    }
}