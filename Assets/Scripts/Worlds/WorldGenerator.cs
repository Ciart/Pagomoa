using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Worlds
{
    [RequireComponent(typeof(WorldManager))]
    public class WorldGenerator : MonoBehaviour
    {
        public WorldDatabase database;

        public int chunkSize = 16;

        public int top = 4;

        public int bottom = 16;

        public int left = 4;

        public int right = 4;

        public Wall wall;

        public Ground ground;


        private WorldManager _worldManager;

        private List<(float, Piece)> _weightedPieces;

        private void Awake()
        {
            _worldManager = GetComponent<WorldManager>();

            Generate();
        }

        private void Preload()
        {
            var pieces = database.pieces;

            float rarityCount = pieces.Sum(piece => piece.rarity);

            _weightedPieces = new List<(float, Piece)>();

            foreach (var piece in pieces)
            {
                _weightedPieces.Add((piece.rarity / rarityCount, piece));
            }

            _weightedPieces.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        }

        private Piece GetRandomPiece()
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

            var world = new World(chunkSize, top, bottom, left, right);

            for (var i = -left * chunkSize; i <= right * chunkSize; i++)
            {
                for (var j = -bottom * chunkSize; j <= top * chunkSize; j++)
                {
                    if (j > 0)
                    {
                        continue;
                    }

                    var worldBrick2 = world.GetBrick(new Vector2Int(i, j));

                    if (worldBrick2 != null)
                    {
                        worldBrick2.wall = wall;
                        worldBrick2.ground = ground;
                    }

                    // if (Random.Range(0, 30) != 0)
                    // {
                    //     continue;
                    // }
                    //
                    // var piece = GetRandomPiece();
                    //
                    // for (var x = 0; x < piece.width; x++)
                    // {
                    //     for (var y = 0; y < piece.height; y++)
                    //     {
                    //         var coordinates = new Vector2Int(i + x, j + y) - piece.pivot;
                    //
                    //         var worldBrick = world.GetBrick(coordinates);
                    //
                    //         if (worldBrick == null)
                    //         {
                    //             continue;
                    //         }
                    //
                    //         piece.GetBrick(x, y).CopyTo(worldBrick);
                    //         worldBrick.wall = wall;
                    //     }
                    // }
                }
            }

            _worldManager.world = world;
        }
    }
}