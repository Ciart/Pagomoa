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
            var point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -_camera.transform.position.z));
        
            if(Input.GetMouseButtonDown(0))
            {
                var tile = GetTile(groundTilemap.layoutGrid.WorldToCell(point));
                Debug.Log(tile.strength);
            }
        
            if(Input.GetMouseButtonDown(1))
            {
                BreakTile(groundTilemap.layoutGrid.WorldToCell(point), debugTier);
            }

        }

        public DiggableTile GetTile(Vector3Int position)
        {
            return groundTilemap.GetTile<DiggableTile>(position);
        }

        public void SetBrick(Brick brick, Vector2Int position)
        {
            _map[position.x, position.y] = brick;
            groundTilemap.SetTile(new Vector3Int(position.x, -position.y, 0), brick.ground.tile);
            mineralTilemap.SetTile(new Vector3Int(position.x, -position.y, 0), brick.mineral.tile);
        }

        public void BreakTile(Vector3Int position, int tier = 10)
        {
            var mineralTile = mineralTilemap.GetTile<MineralTile>(position);

            if (mineralTile is not null && mineralTile.data.tier <= tier)
            {
                var entity = Instantiate(mineralEntity, mineralTilemap.layoutGrid.GetCellCenterWorld(position), Quaternion.identity);
                entity.Data = mineralTile.data;
            }

            groundTilemap.SetTile(position, null);
            mineralTilemap.SetTile(position, null);
        }
    }
}
