using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maps
{
    [RequireComponent(typeof(MapManager))]
    public class MapGenerator : MonoBehaviour
    {
        public MapDatabase database;
        
        public int width = 64;
        public int height = 64;

        private MapManager _mapManager;

        private List<(float, Piece)> _weightedPieces;

        private void Awake()
        {
            _mapManager = GetComponent<MapManager>();
        }

        public void Preload()
        {
            var pieces = database.pieces;
            
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
            _mapManager.ResetMap(width, height, database.GetGround("Dirt"));
            
            Preload();
            
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (Random.Range(0, 30) != 0)
                    {
                        continue;
                    }

                    var piece = GetRandomPiece();

                    for (var x = 0; x < piece.width; x++)
                    {
                        for (var y = 0; y < piece.height; y++)
                        {
                            var position = new Vector2Int(i + x, j + y) - piece.Pivot;
                            _mapManager.SetBrick(piece.GetBrick(x, y), position);
                        }
                    }
                }
            }
        }
    }
}
