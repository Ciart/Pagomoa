using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Brick : MonoBehaviour
{
    public Tilemap groundTilemap;

    public Tilemap mineralTilemap;

    public MineralEntity mineralEntity;

    public DiggableTile GetTile(Vector3Int position)
    {
        return groundTilemap.GetTile<DiggableTile>(position);
    }
    public void MakeDot(Vector3 Pos, int tier = 0)
    {
        Vector3Int cellPosition = groundTilemap.WorldToCell(Pos);
        
        var mineralTile = mineralTilemap.GetTile<MineralTile>(cellPosition);

        if (mineralTile is not null && mineralTile.data.tier <= tier)
        {
            var entity = Instantiate(mineralEntity, mineralTilemap.layoutGrid.GetCellCenterWorld(cellPosition), Quaternion.identity);
            entity.Data = mineralTile.data;
        }

        mineralTilemap.SetTile(cellPosition, null);
        groundTilemap.SetTile(cellPosition, null);
    }
}

