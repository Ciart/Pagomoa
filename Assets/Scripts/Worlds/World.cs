using System;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds
{
    [Serializable]
    public class Chunk
    {
        public Vector2Int key;

        public Brick[] bricks;

        public readonly List<WorldPrefab> prefabs;

        private readonly int _size;

        public Chunk(Vector2Int key, int size)
        {
            this.key = key;
            bricks = new Brick[size * size];
            prefabs = new List<WorldPrefab>();
            _size = size;

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    bricks[i + j * size] = new Brick();
                }
            }
        }

        private bool CheckRange(float x, float y)
        {
            return 0 <= x && x < _size && 0 <= y && y < _size;
        }
    }
    public class World
    {
        public readonly int chunkSize;

        public readonly int top;

        public readonly int bottom;

        public readonly int left;

        public readonly int right;

        public readonly int groundHeight = 0;

        private readonly Dictionary<Vector2Int, Chunk> _chunks;

        public World(int chunkSize, int top, int bottom, int left, int right)
        {
            this.chunkSize = chunkSize;
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
                    _chunks[key] = new Chunk(key, this.chunkSize);
                }
            }
        }
        public World(WorldData worldData)
        {
            this.chunkSize = worldData.chunkSize;
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
            var key = new Vector2Int(Mathf.FloorToInt((float)x / chunkSize), Mathf.FloorToInt((float)y / chunkSize));

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

            var brinkX = x < 0 ? chunkSize - 1 + (x + 1) % chunkSize  : x % chunkSize;
            var brinkY = y < 0 ? chunkSize - 1 + (y + 1) % chunkSize : y % chunkSize;
            
            return chunk.bricks[brinkX + brinkY * chunkSize];
        }

        public void AddPrefab(float x, float y, GameObject prefab)
        {
            var chunk = GetChunk(Mathf.FloorToInt(x), Mathf.FloorToInt(y));

            if (chunk is null)
            {
                return;
            }
            
            chunk.prefabs.Add(new WorldPrefab(x - chunk.key.x * chunkSize, y - chunk.key.y * chunkSize, prefab));
        }
    }
}