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

        private WorldManager _worldManager;

        private PlayerController _player;

        private Chunk _currentChunk;

        private HashSet<Chunk> _renderingChunk = new();

        private void ClearWorld()
        {
            wallTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
            mineralTilemap.ClearAllTiles();

            _currentChunk = null;
            _renderingChunk = new HashSet<Chunk>();
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

        private void UpdateChunk(Chunk chunk)
        {
            var world = _worldManager.world;

            for (var i = 0; i < world.chunkSize; i++)
            {
                for (var j = 0; j < world.chunkSize; j++)
                {
                    var brick = chunk.bricks[i + j * world.chunkSize];

                    if (brick == null)
                    {
                        continue;
                    }
                    
                    var position = new Vector3Int(chunk.key.x * world.chunkSize + i, chunk.key.y * world.chunkSize + j);

                    wallTilemap.SetTile(position, brick.wall ? brick.wall.tile : null);
                    groundTilemap.SetTile(position, brick.ground ? brick.ground.tile : null);
                    mineralTilemap.SetTile(position,brick.mineral ? brick.mineral.tile : null);
                }
            }
        }

        private void OnCreatedWorld(World world)
        {
            ClearWorld();
            NewMethod();
        }

        private void OnChangedChunk(Chunk chunk)
        {
            if (!_renderingChunk.Contains(chunk))
            {
                return;
            }

            UpdateChunk(chunk);
        }

        private void Awake()
        {
            _worldManager = WorldManager.instance;
            _player = (PlayerController)FindObjectOfType(typeof(PlayerController));

            _worldManager.createdWorld += OnCreatedWorld;
            _worldManager.changedChunk += OnChangedChunk;
            
            ClearWorld();
            NewMethod();
        }

        private void LateUpdate()
        {
            NewMethod();
        }

        private void NewMethod()
        {
            var chunk = _worldManager.GetChunkToPosition(_player.transform.position);

            if (chunk == null || _currentChunk == chunk)
            {
                return;
            }

            _currentChunk = chunk;

            var a = 2;
            var hash = new HashSet<Chunk>();

            for (var i = chunk.key.x - a; i <= chunk.key.x + a; i++)
            {
                for (var j = chunk.key.y - a; j <= chunk.key.y + a; j++)
                {
                    var c = _worldManager.world.GetChunk(new Vector2Int(i, j));

                    if (c == null)
                    {
                        continue;
                    }
                    
                    hash.Add(c);
                }
            }

            foreach (var clearChunk in _renderingChunk.Except(hash))
            {
                ClearChunk(clearChunk);
            }

            foreach (var updateChunk in hash.Except(_renderingChunk))
            {
                UpdateChunk(updateChunk);
            }

            _renderingChunk = hash;
        }
    }
}