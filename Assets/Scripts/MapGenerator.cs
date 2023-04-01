using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

public struct MapTile
{
    public DiggableTile Ground;

    public MineralTile Mineral;
}

[RequireComponent(typeof(MapManager))]
public class MapGenerator : MonoBehaviour
{
    public int width = 64;
    public int height = 64;

    public DiggableTile[] tiles;
    
    public MineralTile[] minerals;

    private MapTile[,] _map;
    
    private MapManager _mapManager;

    private void Awake()
    {
        _mapManager = GetComponent<MapManager>();
    }

    public void Generate()
    {
        _map = new MapTile[width, height];

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                _map[i, j] = new MapTile() {Ground = tiles[Random.Range(0, tiles.Length)], Mineral = minerals[Random.Range(0, minerals.Length)]};
                _mapManager.groundTilemap.SetTile(new Vector3Int(i, -j, 0), _map[i, j].Ground);
                _mapManager.mineralTilemap.SetTile(new Vector3Int(i, -j, 0), _map[i, j].Mineral);
            }
        }
    }
}
