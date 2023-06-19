using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maps
{
    [RequireComponent(typeof(WorldManager))]
    public class WorldGenerator : MonoBehaviour
    {
        public WorldDatabase database;

        public int width = 64;
        public int height = 64;

        private WorldManager _worldManager;

        private List<(float, Piece)> _weightedPieces;

        private void Awake()
        {
            _worldManager = GetComponent<WorldManager>();

            Generate();
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
            Preload();

            var world = new World(16, 4, 8, 4, 4);

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
                            
                            ref var brick = ref world.GetBrick(position);
                            brick = piece.GetBrick(x, y);
                        }
                    }
                }
            }
        }
    }
}