using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maps
{
    [RequireComponent(typeof(MapManager))]
    public class MapGenerator : MonoBehaviour
    {
        public int width = 64;
        public int height = 64;

        public Piece[] pieces;
    
        private MapManager _mapManager;

        private List<(float, Piece)> _weightedPieces;

        private void Awake()
        {
            _mapManager = GetComponent<MapManager>();
        }

        public void Preload()
        {
            float rarityCount = pieces.Sum(piece => piece.Rarity);

            _weightedPieces = new List<(float, Piece)>();
        
            foreach (var piece in pieces)
            {
                _weightedPieces.Add((piece.Rarity / rarityCount, piece));
            }
        
            _weightedPieces.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        }

        public Piece GetRandomPiece()
        {
            var pivot = Random.value;

            var count = 0f;

            foreach (var (weight, piece) in _weightedPieces)
            {
                count += weight;

                if (pivot <= count)
                {
                    return piece;
                }
            }

            return _weightedPieces.Last().Item2;
        }

        public void Generate()
        {
            // _map = new MapTile[width, height];
            //
            // for (var i = 0; i < width; i++)
            // {
            //     for (var j = 0; j < height; j++)
            //     {
            //         _map[i, j] = new MapTile() {Ground = tiles[Random.Range(0, tiles.Length)], Mineral = minerals[Random.Range(0, minerals.Length)]};
            //         _mapManager.groundTilemap.SetTile(new Vector3Int(i, -j, 0), _map[i, j].Ground);
            //         _mapManager.mineralTilemap.SetTile(new Vector3Int(i, -j, 0), _map[i, j].Mineral);
            //     }
            // }
        }
    }
}
