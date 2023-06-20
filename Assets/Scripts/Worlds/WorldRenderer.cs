using System;
using System.Collections.Generic;
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

        private HashSet<Chunk> _renderingChunk = new();

        private void Awake()
        {
            _worldManager = WorldManager.instance;
            _player = (PlayerController)FindObjectOfType(typeof(PlayerController));

            _worldManager.changedChunk += OnChangedChunk;
        }

        private void LateUpdate()
        {
            
        }
        
        private void OnChangedChunk(Chunk chunk)
        {
            if (!_renderingChunk.Contains(chunk))
            {
                return;
            }

            UpdateChunk(chunk);
        }

        public void Clear()
        {
            wallTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
            mineralTilemap.ClearAllTiles();
        }
    }
}