using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds
{
    public class Level
    {
        public readonly string id;

        public readonly LevelType type;

        public readonly int top;

        public readonly int bottom;

        public readonly int left;

        public readonly int right;

        public List<EntityData> entityDataList;

        private readonly Dictionary<ChunkCoords, Chunk> _chunks;

        public WorldBounds bounds => WorldBounds.FromTopBottomLeftRight(top, bottom, left, right);

        public Level(string id, LevelType type, int top, int bottom, int left, int right)
        {
            this.id = id;
            this.type = type;
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;

            entityDataList = new List<EntityData>();

            _chunks = new Dictionary<ChunkCoords, Chunk>();

            foreach (var key in bounds.GetChunkKeys())
            {
                _chunks[key] = new Chunk(key);
            }
        }

        public Level(LevelSaveData saveData)
        {
            top = saveData.top;
            bottom = saveData.bottom;
            left = saveData.left;
            right = saveData.right;

            entityDataList = saveData.entities.Select(entity => new EntityData(entity)).ToList();

            _chunks = new Dictionary<ChunkCoords, Chunk>();

            foreach (var chunkData in saveData.chunks)
            {
                _chunks.Add(chunkData.coords, new Chunk(chunkData));
            }
        }

        public LevelSaveData CreateSaveData()
        {
            RefreshEntityData();

            return new LevelSaveData()
            {
                id = id,
                type = type,
                top = top,
                bottom = bottom,
                left = left,
                right = right,
                entities = entityDataList.Select(entity => entity.CreateSaveData()).ToArray(),
                chunks = _chunks.Values.Select(chunk => chunk.CreateSaveData()).ToArray()
            };
        }

        public Chunk? GetChunk(ChunkCoords coords)
        {
            return _chunks.TryGetValue(coords, out var chunk) ? chunk : null;
        }

        public Chunk? GetChunk(int x, int y)
        {
            var key = new ChunkCoords(Mathf.FloorToInt((float)x / Chunk.Size), Mathf.FloorToInt((float)y / Chunk.Size));

            return GetChunk(key);
        }

        public IEnumerable<Chunk> GetNeighborChunks(ChunkCoords coords)
        {
            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    var chunk = GetChunk(coords + new ChunkCoords(i, j));

                    if (chunk is null)
                    {
                        continue;
                    }

                    yield return chunk;
                }
            }
        }

        public Dictionary<ChunkCoords, Chunk> GetAllChunks()
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

        public void AddEntityData(float x, float y, string entityId, EntityStatus status = null)
        {
            entityDataList.Add(new EntityData(entityId, x, y, status));
        }

        public void RefreshEntityData()
        {
            var entityManager = Game.Instance.Entity;
            var dataList = new List<EntityData>();

            foreach (var entityController in entityManager.GetEntitiesInLevel(id))
            {
                if (entityController.isDead)
                {
                    continue;
                }

                var data = entityController.GetEntityData();

                dataList.Add(data);
            }

            entityDataList = dataList;
        }

        public void SpawnEntities()
        {
            var entityManager = Game.Instance.Entity;

            foreach (var entityData in entityDataList)
            {
                var position = new Vector3(entityData.x, entityData.y);
                var coords = WorldManager.ComputeCoords(position);

                if (!bounds.Contains(coords))
                {
                    continue;
                }

                entityManager.Spawn(entityData.id, position, levelId: id);
            }
        }

        public void DespawnEntities()
        {
            RefreshEntityData();
            Game.Instance.Entity.DespawnInLevel(id);
        }
    }
}
