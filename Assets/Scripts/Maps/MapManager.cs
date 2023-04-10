using System;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maps
{
    public class MapManager : MonoBehaviour
    {
        public Tilemap groundTilemap;

        public Tilemap mineralTilemap;

        public MineralEntity mineralEntity;

        public int debugTier;

        // private static MapManager _instance = null;
        private Camera _camera;

        private Brick[,] _map;


        // public static MapManager Instance
        // {
        //     get
        //     {
        //         if (_instance == null)
        //         {
        //             var obj = new GameObject(nameof(MapManager));
        //             _instance = obj.AddComponent<MapManager>();
        //         }
        //         
        //         return _instance;
        //     }
        // }

        private void Awake()
        {
            _camera = Camera.main;
            // _instance = this;
            // DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            var point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                -_camera.transform.position.z));

            if (Input.GetMouseButtonDown(0))
            {
                var tile = GetTile(groundTilemap.layoutGrid.WorldToCell(point));
                Debug.Log(tile.strength);
            }

            if (Input.GetMouseButtonDown(1))
            {
                BreakTile(groundTilemap.layoutGrid.WorldToCell(point), debugTier);
            }
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

        public DiggableTile GetTile(Vector3Int position)
        {
            return groundTilemap.GetTile<DiggableTile>(position);
        }

        public void SetBrick(Brick brick, Vector2Int position, bool isRemove = false)
        {
            if (0 > position.x || position.x >= _map.GetLength(0) ||
                0 > position.y || position.y >= _map.GetLength(1))
            {
                return;
            }

            ;

            var tilemapPosition = new Vector3Int(position.x, -position.y, 0);

            _map[position.x, position.y] = brick;

            if (brick.ground is not null)
            {
                groundTilemap.SetTile(tilemapPosition, brick.ground.tile);
            }
            else if (isRemove)
            {
                groundTilemap.SetTile(tilemapPosition, null);
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
    }
}