using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds
{
    public class World
    {
        public const int GroundHeight = 0;

        public readonly int top;

        public readonly int bottom;

        public readonly int left;

        public readonly int right;
        
        public readonly List<WorldEntityData> entityDataList;
        
        private readonly Dictionary<Vector2Int, Chunk> _chunks;

        public World(int top, int bottom, int left, int right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;

            entityDataList = new List<WorldEntityData>();
            
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

            entityDataList = worldData.entityDataList;

            _chunks = ListDictionaryConverter.ToDictionary(worldData._chunks);
        }

        public Chunk GetChunk(Vector2Int key)
        {
            return _chunks.TryGetValue(key, out var chunk) ? chunk : null;
        }

        public Chunk GetChunk(int x, int y)
        {
            var key = new Vector2Int(Mathf.FloorToInt((float)x / Chunk.Size), Mathf.FloorToInt((float)y / Chunk.Size));

            return GetChunk(key);
        }

        public IEnumerable<Chunk> GetNeighborChunks(Vector2Int key)
        {
            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    var chunk = GetChunk(key + new Vector2Int(i, j));

                    if (chunk is null)
                    {
                        continue;
                    }

                    yield return chunk;
                }
            }
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

            var brinkX = x < 0 ? Chunk.Size - 1 + (x + 1) % Chunk.Size : x % Chunk.Size;
            var brinkY = y < 0 ? Chunk.Size - 1 + (y + 1) % Chunk.Size : y % Chunk.Size;

            return chunk.bricks[brinkX + brinkY * Chunk.Size];
        }

        public void AddEntity(float x, float y, Entity entity, EntityStatus status = null)
        {
            entityDataList.Add(new WorldEntityData(x, y, entity, status));
        }

        public void AddEntity(WorldEntityData entityData)
        {
            entityDataList.Add(entityData);
        }
    }
}