using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Worlds
{
    public class WorldRenderer : MonoBehaviour
    {
        public Tilemap wallTilemap;

        public Tilemap groundTilemap;

        public Tilemap mineralTilemap;

        [Range(1, 16)] public int renderChunkRange = 2;

        private WorldManager _worldManager;

        private PlayerController _player;

        private Chunk _currentChunk;

        private HashSet<Chunk> _renderedChunks = new();

        private void ClearWorld()
        {
            wallTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
            mineralTilemap.ClearAllTiles();

            _currentChunk = null;
            _renderedChunks = new HashSet<Chunk>();
        }

        private void ClearChunk(Chunk chunk)
        {
            var world = _worldManager.world;

            // TODO: 리팩토링 해야 함
            for (var i = 0; i < world.chunkSize; i++)
            {
                for (var j = 0; j < world.chunkSize; j++)
                {
                    var position = new Vector3Int(chunk.key.x * world.chunkSize + i, chunk.key.y * world.chunkSize + j);

                    wallTilemap.SetTile(position, null);
                    groundTilemap.SetTile(position, null);
                    mineralTilemap.SetTile(position, null);
                }
            }
        }

        private void RenderChunk(Chunk chunk)
        {
            var world = _worldManager.world;

            for (var i = 0; i < world.chunkSize; i++)
            {
                for (var j = 0; j < world.chunkSize; j++)
                {
                    var brick = chunk.bricks[i + j * world.chunkSize];

                    var position = new Vector3Int(chunk.key.x * world.chunkSize + i,
                        chunk.key.y * world.chunkSize + j);

                    wallTilemap.SetTile(position, brick.wall ? brick.wall.tile : null);
                    groundTilemap.SetTile(position, brick.ground ? brick.ground.tile : null);
                    mineralTilemap.SetTile(position, brick.mineral ? brick.mineral.tile : null);
                }
            }
        }

        private void OnCreatedWorld(World world)
        {
            ClearWorld();
            RenderWorld();
        }

        private void OnChangedChunk(Chunk chunk)
        {
            if (!_renderedChunks.Contains(chunk))
            {
                return;
            }

            RenderChunk(chunk);
        }

        private void Awake()
        {
            _worldManager = WorldManager.instance;
            _player = (PlayerController)FindObjectOfType(typeof(PlayerController));

            _worldManager.createdWorld += OnCreatedWorld;
            _worldManager.changedChunk += OnChangedChunk;

            ClearWorld();
            RenderWorld();
        }

        private void LateUpdate()
        {
            RenderWorld();
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
            
            var playerCoord = WorldManager.ComputeCoords(_player.transform.position);
            var playerChunk = world.GetChunk(playerCoord.x, playerCoord.y);

            DrawChunkRectangle(playerChunk, world.chunkSize, Color.cyan);

            if (playerChunk is null || _currentChunk == playerChunk)
            {
                return;
            }

            _currentChunk = playerChunk;

            var renderedChunks = new HashSet<Chunk>();

            for (var keyX = playerChunk.key.x - renderChunkRange; keyX <= playerChunk.key.x + renderChunkRange; keyX++)
            {
                for (var keyY = playerChunk.key.y - renderChunkRange; keyY <= playerChunk.key.y + renderChunkRange; keyY++)
                {
                    var chuck = world.GetChunk(new Vector2Int(keyX, keyY));

                    if (chuck == null)
                    {
                        continue;
                    }

                    renderedChunks.Add(chuck);
                }
            }

            foreach (var clearChunk in _renderedChunks.Except(renderedChunks))
            {
                ClearChunk(clearChunk);
            }

            foreach (var updateChunk in renderedChunks.Except(_renderedChunks))
            {
                RenderChunk(updateChunk);
            }

            _renderedChunks = renderedChunks;
        }
    }
}