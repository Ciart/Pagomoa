using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
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

        [Tooltip("땅 파괴 애니메이션을 표현하는 타일맵입니다.")] public Tilemap groundOverlayTilemap;

        public Tilemap mineralTilemap;

        public Tilemap fogTilemap;

        public Tilemap overlayTilemap;

        public TileBase fogTile;

        public int sight = 2;

        public SpriteRenderer minimapRenderer;

        [Range(1, 256)] public int renderChunkRange = 2;

        /// <summary>
        /// 청크 로딩 중인지 여부입니다.
        /// </summary>
        public bool IsLoading { get; private set; }

        private Chunk _lastPlayerChunk;

        private HashSet<ChunkCoords> _activedChunks = new();

        private Dictionary<ChunkCoords, SpriteRenderer> _minimapRenderers = new();

        private GameObject? minimapObjects;

        private Coroutine _chunkUpdateCoroutine;

        private Coroutine _chunkClearCoroutine;

        public void Init(Level level)
        {
            this.level = level;

            UpdateLevel();
            SpawnEntities();
        }

        private void ClearLevel()
        {
            wallTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
            mineralTilemap.ClearAllTiles();
            fogTilemap.ClearAllTiles();

            _lastPlayerChunk = null;
            _activedChunks = new HashSet<ChunkCoords>();
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

            _activedChunks.Remove(coords);

            var entityManager = EntityManager.instance;

            var world = WorldManager.world;
            var chunk = world.currentLevel.GetChunk(coords) ?? new Chunk(coords);

            foreach (var entityController in entityManager.FindAllEntityInChunk(chunk))
            {
                var position = entityController.transform.position;

                world.currentLevel.AddEntity(position.x, position.y, entityController.entityId);

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
            return brick.ground == null;
        }

        private bool[,] CreateFogMap(Chunk chunk, World world)
        {
            var fogMap = new bool[Chunk.Size, Chunk.Size];

            for (var i = -sight; i < Chunk.Size + sight; i++)
            {
                for (var j = -sight; j < Chunk.Size + sight; j++)
                {
                    var brick = world.currentLevel.GetBrick(chunk.coords.x * Chunk.Size + i,
                        chunk.coords.y * Chunk.Size + j,
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

        private Chunk? GetPlayerChunk()
        {
            if (level is null)
            {
                return null;
            }

            var playerCoord = WorldManager.ComputeCoords(Game.Instance.player?.transform.position ?? Vector3.zero);
            var playerChunk = level.GetChunk(playerCoord.x, playerCoord.y);

            return playerChunk;
        }

        private IEnumerator RunActionWithChunks(IEnumerable<ChunkCoords> keys, Action<ChunkCoords> action, Action? onComplete = null)
        {
            foreach (var key in keys)
            {
                action(key);
                yield return null;
            }

            onComplete?.Invoke();
        }

        private void UpdateEdgeFog()
        {
            var top = level.top;
            var bottom = -level.bottom;
            var left = -level.left - 1;
            var right = level.right;
            for (int i = left; i <= right; i++)
            {
                fogTilemap.SetTile(new Vector3Int(i, top), fogTile);
                fogTilemap.SetTile(new Vector3Int(i, top - 1), fogTile);
                fogTilemap.SetTile(new Vector3Int(i, bottom), fogTile);
                fogTilemap.SetTile(new Vector3Int(i, bottom + 1), fogTile);
            }

            for (int i = bottom; i <= top; i++)
            {
                fogTilemap.SetTile(new Vector3Int(left, i), fogTile);
                fogTilemap.SetTile(new Vector3Int(left + 1, i), fogTile);
                fogTilemap.SetTile(new Vector3Int(right, i), fogTile);
                fogTilemap.SetTile(new Vector3Int(right - 1, i), fogTile);
            }
        }

        private void UpdateChunk(ChunkCoords coords)
        {
            var world = WorldManager.world;
            var texture = new Texture2D(Chunk.Size, Chunk.Size);
            var chunk = world.currentLevel.GetChunk(coords);

            if (chunk is null)
            {
                return;
            }

            var fogMap = CreateFogMap(chunk, world);

            var positions = new Vector3Int[Chunk.Size * Chunk.Size];

            var wallTiles = new TileBase?[positions.Length];
            var groundTiles = new TileBase?[positions.Length];
            var mineralTiles = new TileBase?[positions.Length];
            var fogTiles = new TileBase?[positions.Length];
            var overlayTiles = new TileBase?[positions.Length];

            foreach (var (x, y, index) in chunk.GetBrickPositionsAndIndices())
            {
                var brick = chunk.bricks[index];

                positions[index] = new Vector3Int(chunk.coords.x * Chunk.Size + x,
                    chunk.coords.y * Chunk.Size + y);

                wallTiles[index] = brick.wall?.tile;
                groundTiles[index] = brick?.ground?.tile;
                mineralTiles[index] = brick?.mineral?.tile;
                fogTiles[index] = fogMap[x, y] ? null : fogTile;
                overlayTiles[index] = brick?.mineral != null && !fogMap[x, y]
                    ? WorldManager.instance.database.glitterTile
                    : null;

                var color = Color.clear;

                if (brick.ground != null)
                {
                    ColorUtility.TryParseHtmlString(brick.ground.color, out color);
                }

                texture.SetPixel(x, y, color);
            }

            wallTilemap.SetTiles(positions, wallTiles);
            groundTilemap.SetTiles(positions, groundTiles);
            mineralTilemap.SetTiles(positions, mineralTiles);
            fogTilemap.SetTiles(positions, fogTiles);
            overlayTilemap.SetTiles(positions, overlayTiles);

            _activedChunks.Add(coords);

            texture.Apply();
            texture.filterMode = FilterMode.Point;

            var sprite = Sprite.Create(texture, Rect.MinMaxRect(0f, 0f, Chunk.Size, Chunk.Size),
                Vector2.zero, 1f);

            if (minimapObjects is null)
            {
                minimapObjects = new GameObject();
                minimapObjects.transform.parent = transform;
                minimapObjects.name = "minimap objects";
            }

            if (!_minimapRenderers.TryGetValue(chunk.coords, out var spriteRenderer))
            {
                spriteRenderer = Instantiate(minimapRenderer,
                    new Vector3(chunk.coords.x * Chunk.Size, chunk.coords.y * Chunk.Size), quaternion.identity,
                    minimapObjects.transform);
                _minimapRenderers.Add(chunk.coords, spriteRenderer);
            }

            spriteRenderer.sprite = sprite;
        }

        public void UpdateLevel()
        {
            var playerChunk = GetPlayerChunk();

            if (playerChunk is null || playerChunk == _lastPlayerChunk)
            {
                return;
            }

            _lastPlayerChunk = playerChunk;

            var activeChunks = new HashSet<ChunkCoords>();

            for (var keyX = playerChunk.coords.x - renderChunkRange; keyX <= playerChunk.coords.x + renderChunkRange; keyX++)
            {
                for (var keyY = playerChunk.coords.y - renderChunkRange;
                     keyY <= playerChunk.coords.y + renderChunkRange;
                     keyY++)
                {
                    activeChunks.Add(new ChunkCoords(keyX, keyY));
                }
            }

            var chunksToClear = _activedChunks.Except(activeChunks).ToList();
            var chunksToActivate = activeChunks.Except(_activedChunks).ToList();

            if (_chunkUpdateCoroutine != null)
            {
                StopCoroutine(_chunkUpdateCoroutine);
            }

            if (_chunkClearCoroutine != null)
            {
                StopCoroutine(_chunkClearCoroutine);
            }

            IsLoading = true;

            _chunkClearCoroutine = StartCoroutine(RunActionWithChunks(chunksToClear, ClearChunk));
            _chunkUpdateCoroutine = StartCoroutine(RunActionWithChunks(chunksToActivate, UpdateChunk, () =>
            {
                IsLoading = false;
            }));

            UpdateEdgeFog();
        }

        private void UpdateBrokenEffect()
        {
            var worldManager = Game.Instance.World;
            var brokenTiles = worldManager.database.brokenEffectTiles;

            groundOverlayTilemap.ClearAllTiles();

            foreach (var (key, value) in worldManager.brickDamage)
            {
                var position = new Vector3Int(key.x, key.y, 0);
                var brokenStep = Mathf.FloorToInt((1 - value.health / value.maxHealth) * (brokenTiles.Length - 1));

                groundOverlayTilemap.SetTile(position, brokenTiles[brokenStep]);
            }
        }

        private void LateUpdate()
        {
            UpdateLevel();
            UpdateBrokenEffect();
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

                _entities.Add(entityManager.Spawn(entityData.id, position));
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

        private void OnChunkChanged(ChunkChangedEvent e)
        {
            if (!_activedChunks.Contains(e.chunk.coords))
            {
                return;
            }

            if (e.level != level)
            {
                return;
            }

            UpdateChunk(e.chunk.coords);
            UpdateEdgeFog();
        }

        private void OnEnable()
        {
            EventManager.AddListener<ChunkChangedEvent>(OnChunkChanged);
            UpdateLevel();
            SpawnEntities();
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ChunkChangedEvent>(OnChunkChanged);
            DespawnEntities();
        }
    }
}
