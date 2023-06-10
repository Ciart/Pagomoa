using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maps
{
    public class MapRenderer : MonoBehaviour
    {
        public Tilemap backgroundTilemap;

        public Tilemap groundTilemap;
        
        public Tilemap mineralTilemap;
        
        public Tile backgroundTile;

        public void Clear()
        {
            backgroundTilemap.ClearAllTiles();
            groundTilemap.ClearAllTiles();
            mineralTilemap.ClearAllTiles();
        }

        public void UpdateTile(Brick brick, Vector2Int position)
        {
            
        }
    }
}