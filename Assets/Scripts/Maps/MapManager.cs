using System;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maps
{
    public class MapManager : MonoBehaviour
    {
        public float groundHeight = 0;
        
        public MineralEntity mineralEntity;

        private static MapManager _instance = null;

        private Brick[,] _map;

        public static MapManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (MapManager)FindObjectOfType(typeof(MapManager));
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ResetMap(int width, int height, Ground ground)
        {
            _map = new Brick[width, height];

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    SetBrick(new Brick() { ground = ground }, new Vector2Int(i, j), true);
                }
            }
        }

        public Brick GetBrick(Vector3Int position)
        {
            position.y = -position.y;
            
            if (0 > position.x || position.x >= _map.GetLength(0) ||
                0 > position.y || position.y >= _map.GetLength(1))
            {
                return new Brick();
            }
            
            return _map[position.x, position.y];
        }

        public void SetBrick(Brick brick, Vector2Int position, bool isRemove = false)
        {
            if (0 > position.x || position.x >= _map.GetLength(0) ||
                0 > position.y || position.y >= _map.GetLength(1))
            {
                return;
            }

            var tilemapPosition = new Vector3Int(position.x, -position.y, 0);

            _map[position.x, position.y] = brick;

            if (brick.ground is not null)
            {
                groundTilemap.SetTile(tilemapPosition, brick.ground.tile);
                backgroundTilemap.SetTile(tilemapPosition, backgroundTile);
            }
            else if (isRemove)
            {
                groundTilemap.SetTile(tilemapPosition, null);
                backgroundTilemap.SetTile(tilemapPosition, null);
            }

            if (brick.mineral is not null)
            {
                mineralTilemap.SetTile(tilemapPosition, brick.mineral.tile);
            }
            else if (isRemove)
            {
                mineralTilemap.SetTile(tilemapPosition, null);
            }
        }

        public void BreakTile(Vector3Int position, int tier = 10)
        {
            var mineralTile = mineralTilemap.GetTile<MineralTile>(position);

            if (mineralTile is not null && mineralTile.data.tier <= tier)
            {
                var entity = Instantiate(mineralEntity, mineralTilemap.layoutGrid.GetCellCenterWorld(position),
                    Quaternion.identity);
                entity.Data = mineralTile.data;
            }

            groundTilemap.SetTile(position, null);
            mineralTilemap.SetTile(position, null);
        }
        
        public bool CheckClimbable(Vector3 position)
        {
            var tile = backgroundTilemap.GetTile(Vector3Int.FloorToInt(position));

            if (tile is null)
            {
                return false;
            }

            return true;
        }
    }
}