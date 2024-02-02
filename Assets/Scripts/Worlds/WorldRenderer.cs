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
    public class WorldRenderer : MonoBehaviour
    {
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

        private WorldManager _worldManager;

        private EntityManager _entityManager;

        private Chunk _currentChunk;

        private HashSet<Vector2Int> _renderedChunks = new();

        private Dictionary<Vector2Int, SpriteRenderer> _minimapRenderers = new();

        private void ClearWorld()
        {
            wallTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
            mineralTilemap.ClearAllTiles();
            fogTilemap.ClearAllTiles();

            _currentChunk = null;
            _renderedChunks = new HashSet<Vector2Int>();
            _minimapRenderers = new Dictionary<Vector2Int, SpriteRenderer>();
        }

        private void ClearChunk(Vector2Int key)
        {
            // TODO: 리팩토링 해야 함
            for (var i = 0; i < Chunk.Size; i++)
            {
                for (var j = 0; j < Chunk.Size; j++)
                {
                    var position = new Vector3Int(key.x * Chunk.Size + i, key.y * Chunk.Size + j);

                    wallTilemap.SetTile(position, null);
                    groundTilemap.SetTile(position, null);
                    mineralTilemap.SetTile(position, null);
                    fogTilemap.SetTile(position, null);
                }
            }

            var chunk = _worldManager.world.GetChunk(key) ?? new Chunk(key);

            foreach (var entityController in _entityManager.FindAllEntityInChunk(chunk))
            {
                var position = entityController.transform.position;

                _worldManager.world.AddEntity(position.x, position.y, entityController.entity);

                _entityManager.Despawn(entityController);
            }

            if (_minimapRenderers.TryGetValue(key, out var value))
            {
                Destroy(value.gameObject);
            }

            _minimapRenderers.Remove(key);
        }

        private bool CheckSightBrick(Brick brick)
        {
            return brick.ground is null ||
                   brick.mineral == _worldManager.database.GetMineral("UFORemote");
        }

        private bool[,] CreateFogMap(Chunk chunk, World world)
        {
            var fogMap = new bool[Chunk.Size, Chunk.Size];

            for (var i = -sight; i < Chunk.Size + sight; i++)
            {
                for (var j = -sight; j < Chunk.Size + sight; j++)
                {
                    var brick = world.GetBrick(chunk.key.x * Chunk.Size + i, chunk.key.y * Chunk.Size + j,
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

        private void RenderChunk(Vector2Int key)
        {
            var world = _worldManager.world;
            var texture = new Texture2D(Chunk.Size, Chunk.Size);
            var chunk = world.GetChunk(key);

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

                    var position = new Vector3Int(chunk.key.x * Chunk.Size + i,
                        chunk.key.y * Chunk.Size + j);

                    wallTilemap.SetTile(position, brick.wall ? brick.wall.tile : null);
                    groundTilemap.SetTile(position, brick.ground ? brick.ground.tile : null);
                    mineralTilemap.SetTile(position, brick.mineral ? brick.mineral.tile : null);
                    fogTilemap.SetTile(position, fogMap[i, j] ? null : fogTile);
                    overlayTilemap.SetTile(position,
                        brick.mineral && !fogMap[i, j] ? _worldManager.database.glitterTile : null);

                    texture.SetPixel(i, j, brick.ground ? brick.ground.color : Color.clear);
                }
            }

            texture.Apply();
            texture.filterMode = FilterMode.Point;

            var sprite = Sprite.Create(texture, Rect.MinMaxRect(0f, 0f, Chunk.Size, Chunk.Size),
                Vector2.zero, 1f);

            if (!_minimapRenderers.TryGetValue(chunk.key, out var spriteRenderer))
            {
                spriteRenderer = Instantiate(minimapRenderer,
                    new Vector3(chunk.key.x * Chunk.Size, chunk.key.y * Chunk.Size), quaternion.identity);
                _minimapRenderers.Add(chunk.key, spriteRenderer);
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

        private void OnWorldCreated(WorldCreatedEvent e)
        {
            ClearWorld();
            RenderWorld();
        }

        private void OnChunkChanged(ChunkChangedEvent e)
        {
            // if (!_renderedChunks.Contains(chunk.key))
            // {
            //     return;
            // }

            RenderChunk(e.chunk.key);
        }

        private void Start()
        {
            _worldManager = WorldManager.instance;
            _entityManager = EntityManager.instance;
        }

        private void OnEnable()
        {
            EventManager.AddListener<WorldCreatedEvent>(OnWorldCreated);
            EventManager.AddListener<ChunkChangedEvent>(OnChunkChanged);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<WorldCreatedEvent>(OnWorldCreated);
            EventManager.RemoveListener<ChunkChangedEvent>(OnChunkChanged);
        }

        private void LateUpdate()
        {
            // RenderWorld();

            groundOverlayTilemap.ClearAllTiles();

            var brokenTiles = _worldManager.database.brokenEffectTiles;

            foreach (var (key, value) in _worldManager.brickDamage)
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

            var position = new Vector3(chunk.key.x * chunkSize, chunk.key.y * chunkSize, 0);

            Debug.DrawLine(position, position + Vector3.right * chunkSize, color);
            Debug.DrawLine(position + Vector3.right * chunkSize, position + new Vector3(chunkSize, chunkSize, 0),
                color);
            Debug.DrawLine(position + new Vector3(chunkSize, chunkSize, 0), position + Vector3.up * chunkSize, color);
            Debug.DrawLine(position + Vector3.up * chunkSize, position, color);
        }

        private void RenderWorld()
        {
            var world = _worldManager.world;

            if (world is null)
            {
                return;
            }

            var playerCoord = WorldManager.ComputeCoords(_entityManager.player.transform.position);
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

            foreach (var key in world.GetAllChunks().Keys)
            {
                RenderChunk(key);
            }

            foreach (var entityData in world.entityDataList)
            {
                var position = new Vector3(entityData.x, entityData.y);
                var coords = WorldManager.ComputeCoords(position);

                if (_worldManager.world.GetBrick(coords.x, coords.y, out _).wall is null)
                {
                    continue;
                }

                _entityManager.Spawn(entityData.entity, position);
            }
        }
    }
}