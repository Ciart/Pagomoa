﻿using System;
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
        public Level? level;

        public Tilemap wallTilemap;

        public Tilemap groundTilemap;

        [Tooltip("땅 파괴 애니메이션을 표현하는 타일맵입니다.")] public Tilemap groundOverlayTilemap;

        public Tilemap mineralTilemap;

        public Tilemap fogTilemap;

        public Tilemap overlayTilemap;

        public TileBase fogTile;

        public int sight = 2;

        public SpriteRenderer minimapRenderer;

        public const int RenderChunkRange = 3;

        /// <summary>
        /// 청크 로딩 중인지 여부입니다.
        /// </summary>
        public bool IsLoading { get; private set; }

        private Chunk _lastPlayerChunk;

        /// <summary>
        /// 렌더링 중인 청크들입니다. Player 주변 청크입니다.
        /// </summary>
        private HashSet<ChunkCoords> _activeChunks = new();

        /// <summary>
        /// _activeChunks에 포함 된 청크들 중, 인접 청크가 모두 렌더링 중인 청크들입니다.
        /// Entity는 Live Chunk 영역 밖에 있을 경우, 시뮬레이션되지 않습니다.
        /// </summary>
        private HashSet<ChunkCoords> _liveChunks = new();

        private Dictionary<ChunkCoords, SpriteRenderer> _minimapRenderers = new();

        private GameObject? minimapObjects;

        private Coroutine _chunkUpdateCoroutine;

        private Coroutine _chunkClearCoroutine;

        public void Init(Level level)
        {
            this.level = level;

            UpdateLevel();
        }

        private void ClearLevel()
        {
            wallTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
            mineralTilemap.ClearAllTiles();
            fogTilemap.ClearAllTiles();

            _lastPlayerChunk = null;
            _activeChunks = new HashSet<ChunkCoords>();
            _liveChunks = new HashSet<ChunkCoords>();
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

            _activeChunks.Remove(coords);
            _liveChunks = ComputeLiveChunks();

            var entityManager = EntityManager.instance;

            var world = WorldManager.world;
            var chunk = world.currentLevel.GetChunk(coords) ?? new Chunk(coords);

            foreach (var entityController in entityManager.FindAllEntityInChunk(level.id, chunk))
            {
                var position = entityController.transform.position;

                world.currentLevel.AddEntityData(position.x, position.y, entityController.entityId);

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

        private void SpawnEntitiesInChunk(Chunk chunk)
        {
            if (level is null)
            {
                Debug.LogError("Level is null");
                return;
            }

            var entityManager = Game.Instance.Entity;

            var entities = level.entityDataList.Where(entityData =>
                entityData.x >= chunk.worldRect.xMin && entityData.x <= chunk.worldRect.xMax &&
                entityData.y >= chunk.worldRect.yMin && entityData.y <= chunk.worldRect.yMax).ToList();

            foreach (var entityData in entities)
            {
                entityManager.Spawn(entityData.id, new Vector3(entityData.x, entityData.y), entityData.status, level.id);
                level.entityDataList.Remove(entityData);
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

            _activeChunks.Add(coords);
            _liveChunks = ComputeLiveChunks();

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

            SpawnEntitiesInChunk(chunk);
        }

        public void UpdateLevel()
        {
            var playerChunk = GetPlayerChunk();

            if (playerChunk is null || playerChunk == _lastPlayerChunk)
            {
                return;
            }

            _lastPlayerChunk = playerChunk;

            var playerNeighborChunks = new HashSet<ChunkCoords>();

            for (var keyX = playerChunk.coords.x - RenderChunkRange; keyX <= playerChunk.coords.x + RenderChunkRange; keyX++)
            {
                for (var keyY = playerChunk.coords.y - RenderChunkRange;
                     keyY <= playerChunk.coords.y + RenderChunkRange;
                     keyY++)
                {
                    playerNeighborChunks.Add(new ChunkCoords(keyX, keyY));
                }
            }

            var chunksToClear = _activeChunks.Except(playerNeighborChunks).ToList();
            var chunksToActivate = playerNeighborChunks.Except(_activeChunks).ToList();

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

        private bool IsChunkActive(ChunkCoords coords)
        {
            return _activeChunks.Contains(coords);
        }

        private bool IsNeighborChunkActive(ChunkCoords coords)
        {
            foreach (var chunk in level.GetNeighborChunks(coords))
            {
                if (!IsChunkActive(coords))
                {
                    return false;
                }
            }

            return true;
        }

        private HashSet<ChunkCoords> ComputeLiveChunks()
        {
            var result = new HashSet<ChunkCoords>();

            foreach (var chunk in _activeChunks)
            {
                if (IsNeighborChunkActive(chunk))
                {
                    result.Add(chunk);
                }
            }

            return result;
        }

        public bool IsLiveChunk(ChunkCoords coords)
        {
            return _liveChunks.Contains(coords);
        }

        public void DespawnEntities()
        {
            if (level is null)
            {
                return;
            }

            level.entityDataList = level.CreateEntitiesData();
            Game.Instance.Entity.DespawnInLevel(level.id);
        }

        private void OnChunkChanged(ChunkChangedEvent e)
        {
            if (!_activeChunks.Contains(e.chunk.coords))
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
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ChunkChangedEvent>(OnChunkChanged);
            DespawnEntities();
        }
    }
}
