using System;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    public class LevelRenderer : MonoBehaviour
    {
        public Level level;
        
        public Tilemap wallTilemap;

        public Tilemap groundTilemap;

        public Tilemap groundOverlayTilemap;

        public Tilemap mineralTilemap;

        public Tilemap fogTilemap;

        public Tilemap overlayTilemap;

        public TileBase fogTile;

        public int sight = 2;

        public SpriteRenderer minimapRenderer;

        [Range(1, 256)] public int renderChunkRange = 2;

        private Chunk _currentChunk;

        private HashSet<ChunkCoords> _renderedChunks = new();

        private Dictionary<ChunkCoords, SpriteRenderer> _minimapRenderers = new();

        private GameObject minimapObjects;

        public void Init(Level level)
        {
            this.level = level;
            
            RenderLevel();
            SpawnEntities();
        }

        private void ClearLevel()
        {
            wallTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
            mineralTilemap.ClearAllTiles();
            fogTilemap.ClearAllTiles();

            _currentChunk = null;
            _renderedChunks = new HashSet<ChunkCoords>();
            _minimapRenderers = new Dictionary<ChunkCoords, SpriteRenderer>();
        }

        private void ClearChunk(ChunkCoords coords)
        {
            // TODO: 리팩토링 해야 함
            for (var i = 0; i < Chunk.Size; i++)
            {
                for (var j = 0; j < Chunk.Size; j++)
                {
                    var position = new Vector3Int(coords.x * Chunk.Size + i, coords.y * Chunk.Size + j);

                    wallTilemap.SetTile(position, null);
                    groundTilemap.SetTile(position, null);
                    mineralTilemap.SetTile(position, null);
                    fogTilemap.SetTile(position, null);
                }
            }

            var entityManager = EntityManager.instance;
            
            var world = WorldManager.world;
            var chunk = world.currentLevel.GetChunk(coords) ?? new Chunk(coords);

            foreach (var entityController in entityManager.FindAllEntityInChunk(chunk))
            {
                var position = entityController.transform.position;

                world.currentLevel.AddEntity(position.x, position.y, entityController.origin);

                entityManager.Despawn(entityController);
            }

            if (_minimapRenderers.TryGetValue(coords, out var value))
            {
                Destroy(value.gameObject);
            }

            _minimapRenderers.Remove(coords);
        }

        private bool CheckSightBrick(Brick brick)
        {
            return brick.ground is null ||
                   brick.mineral == WorldManager.instance.database.GetMineral("UFORemote");
        }

        private bool[,] CreateFogMap(Chunk chunk, World world)
        {
            var fogMap = new bool[Chunk.Size, Chunk.Size];

            for (var i = -sight; i < Chunk.Size + sight; i++)
            {
                for (var j = -sight; j < Chunk.Size + sight; j++)
                {
                    var brick = world.currentLevel.GetBrick(chunk.coords.x * Chunk.Size + i, chunk.coords.y * Chunk.Size + j,
                        out _);

                    if (brick is null || !CheckSightBrick(brick))
                    {
                        continue;
                    }

                    for (var x = i - sight; x <= i + sight; x++)
                    {
                        for (var y = j - sight; y <= j + sight; y++)
                        {
                            if (x < 0 || x >= Chunk.Size || y < 0 || y >= Chunk.Size)
                            {
                                continue;
                            }

                            fogMap[x, y] = true;
                        }
                    }
                }
            }

            return fogMap;
        }

        private void RenderChunk(ChunkCoords coords)
        {
            var world = WorldManager.world;
            var texture = new Texture2D(Chunk.Size, Chunk.Size);
            var chunk = world.currentLevel.GetChunk(coords);

            if (chunk is null)
            {
                // chunk = new Chunk(key, Chunk.Size);
                //
                // foreach (var brick in chunk.bricks)
                // {
                //     brick.ground = _worldManager.database.GetGround("Grass");
                // }

                return;
            }

            var fogMap = CreateFogMap(chunk, world);

            for (var i = 0; i < Chunk.Size; i++)
            {
                for (var j = 0; j < Chunk.Size; j++)
                {
                    var brick = chunk.bricks[i + j * Chunk.Size];

                    var position = new Vector3Int(chunk.coords.x * Chunk.Size + i,
                        chunk.coords.y * Chunk.Size + j);

                    wallTilemap.SetTile(position, brick.wall ? brick.wall.tile : null);
                    groundTilemap.SetTile(position, brick.ground ? brick.ground.tile : null);
                    mineralTilemap.SetTile(position, brick.mineral ? brick.mineral.tile : null);
                    fogTilemap.SetTile(position, fogMap[i, j] ? null : fogTile);
                    overlayTilemap.SetTile(position,
                        brick.mineral && !fogMap[i, j]
                            ? WorldManager.instance.database.glitterTile
                            : null);

                    texture.SetPixel(i, j, brick.ground ? brick.ground.color : Color.clear);
                }
            }

            texture.Apply();
            texture.filterMode = FilterMode.Point;

            var sprite = Sprite.Create(texture, Rect.MinMaxRect(0f, 0f, Chunk.Size, Chunk.Size),
                Vector2.zero, 1f);

            if (minimapObjects == null)
            {
                minimapObjects = new GameObject();
                minimapObjects.transform.parent = transform;
                minimapObjects.name = "minimap objects";
            }

            if (!_minimapRenderers.TryGetValue(chunk.coords, out var spriteRenderer))
            {
                spriteRenderer = Instantiate(minimapRenderer,
                    new Vector3(chunk.coords.x * Chunk.Size, chunk.coords.y * Chunk.Size), quaternion.identity, minimapObjects.transform);
                _minimapRenderers.Add(chunk.coords, spriteRenderer);
            }

            spriteRenderer.sprite = sprite;
        }

        private IEnumerator RunActionWithChunks(IEnumerable<Vector2Int> keys, Action<Vector2Int> action)
        {
            foreach (var key in keys)
            {
                action(key);
                yield return null;
            }
        }
        
        private void OnChunkChanged(ChunkChangedEvent e)
        {
            // if (!_renderedChunks.Contains(chunk.key))
            // {
            //     return;
            // }

            if (e.level != level)
            {
                return;
            }

            RenderChunk(e.chunk.coords);
        }
        
        private void OnEnable()
        {
            EventManager.AddListener<ChunkChangedEvent>(OnChunkChanged);
            RenderLevel();
            SpawnEntities();
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ChunkChangedEvent>(OnChunkChanged);
            DespawnEntities();
        }

        private void LateUpdate()
        {
            // RenderWorld();

            var worldManager = WorldManager.instance;
            
            groundOverlayTilemap.ClearAllTiles();

            var brokenTiles = worldManager.database.brokenEffectTiles;

            foreach (var (key, value) in worldManager.brickDamage)
            {
                var position = new Vector3Int(key.x, key.y, 0);
                var brokenStep = Mathf.FloorToInt((1 - value.health / value.maxHealth) * (brokenTiles.Length - 1));

                groundOverlayTilemap.SetTile(position, brokenTiles[brokenStep]);
            }
        }

        public static void DrawChunkRectangle(Chunk chunk, int chunkSize, Color color)
        {
            if (chunk == null)
            {
                return;
            }

            var position = new Vector3(chunk.coords.x * chunkSize, chunk.coords.y * chunkSize, 0);

            Debug.DrawLine(position, position + Vector3.right * chunkSize, color);
            Debug.DrawLine(position + Vector3.right * chunkSize, position + new Vector3(chunkSize, chunkSize, 0),
                color);
            Debug.DrawLine(position + new Vector3(chunkSize, chunkSize, 0), position + Vector3.up * chunkSize, color);
            Debug.DrawLine(position + Vector3.up * chunkSize, position, color);
        }

        public void RenderLevel()
        {
            if (level is null)
            {
                return;
            }

            // var playerCoord = WorldManager.ComputeCoords(_entityManager.player.transform.position);
            // var playerChunk = world.GetChunk(playerCoord.x, playerCoord.y);
            //
            // DrawChunkRectangle(playerChunk, Chunk.Size, Color.cyan);
            //
            // if (playerChunk is null || _currentChunk == playerChunk)
            // {
            //     return;
            // }
            //
            // _currentChunk = playerChunk;
            //
            // var renderedChunks = new HashSet<Vector2Int>();
            //
            // for (var keyX = playerChunk.key.x - renderChunkRange; keyX <= playerChunk.key.x + renderChunkRange; keyX++)
            // {
            //     for (var keyY = playerChunk.key.y - renderChunkRange;
            //          keyY <= playerChunk.key.y + renderChunkRange;
            //          keyY++)
            //     {
            //         renderedChunks.Add(new Vector2Int(keyX, keyY));
            //     }
            // }
            //
            // var clearChunks = _renderedChunks.Except(renderedChunks);
            // var updateChunks = renderedChunks.Except(_renderedChunks);
            //
            // StartCoroutine(RunActionWithChunks(clearChunks, ClearChunk));
            // StartCoroutine(RunActionWithChunks(updateChunks, RenderChunk));
            //
            // _renderedChunks = renderedChunks;

            foreach (var chunkCoords in level.GetAllChunks().Keys)
            {
                RenderChunk(chunkCoords);
            }
        }
        
        private List<EntityController> _entities = new();

        public void SpawnEntities()
        {
            if (level is null)
            {
                return;
            }

            var entityManager = EntityManager.instance;
            
            foreach (var entityData in level.entityDataList)
            {
                var position = new Vector3(entityData.x, entityData.y);
                var coords = WorldManager.ComputeCoords(position);

                if (!level.bounds.Contains(coords))
                {
                    continue;
                }
                
                _entities.Add(entityManager.Spawn(entityData.origin, position));
            }
        }
        
        public void DespawnEntities()
        {
            if (level is null)
            {
                return;
            }
            
            var entityManager = EntityManager.instance;
            var dataList = new List<EntityData>();
            
            foreach (var entityController in _entities)
            {
                if (entityController.isDead)
                {
                    continue;
                }
                
                var data = entityController.GetEntityData();
                dataList.Add(data);
                
                entityManager.Despawn(entityController);
            }
            
            level.entityDataList = dataList;

            _entities.Clear();
        }
    }
}