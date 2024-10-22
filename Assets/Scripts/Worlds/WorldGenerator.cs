using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Ciart.Pagomoa.Worlds
{
    using WeightedPieces = List<(float, Piece)>;
    
    public class WorldGenerator : MonoBehaviour
    {
        public const int ForestHeight = -100;

        public uint seed = 1234;

        public int chunkSize = 16;

        public int top = 64;

        public int bottom = 128;

        public int left = 64;

        public int right = 64;

        public Wall wall;

        private WorldManager _worldManager;

        private void Awake()
        {
            _worldManager = Game.Get<WorldManager>();
            _worldManager.GetComponent(this);
        }

        private WeightedPieces Preload(IEnumerable<Piece> pieces)
        {
            var weightCount = pieces.Sum(piece => piece.weight);

            var weightedPieces = new WeightedPieces();

            foreach (var piece in pieces)
            {
                weightedPieces.Add((piece.weight / weightCount, piece));
            }

            weightedPieces.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            return weightedPieces;
        }

        private Piece GetRandomPiece(WeightedPieces weightedPieces, Random random)
        {
            var pivot = random.NextFloat();

            var count = 0f;

            foreach (var (weight, piece) in weightedPieces)
            {
                count += weight;

                if (pivot <= count)
                {
                    return piece;
                }
            }

            return weightedPieces.Last().Item2;
        }

        public Level GenerateMainLevel()
        {
            var level = new Level("Main", LevelType.Overworld, top, bottom, left, right);

            var database = _worldManager.database;
            var random = new Random(seed);

            var desertPieces =
                Preload(database.pieces.Where((piece) => piece.appearanceArea.HasFlag(WorldAreaFlag.Desert)));
            var forestPieces =
                Preload(database.pieces.Where((piece) => piece.appearanceArea.HasFlag(WorldAreaFlag.Forest)));

            var sand = database.GetGround("Sand");
            var grass = database.GetGround("Grass");
            
            var levelBounds = level.bounds;

            foreach (var coords in levelBounds.GetWorldCoords())
            {
                if (coords.y >= World.GroundHeight)
                {
                    continue;
                }


                var worldBrick = level.GetBrick(coords.x, coords.y, out _);

                if (worldBrick is not null)
                {
                    worldBrick.wall = wall;

                    if (coords.y > ForestHeight)
                    {
                        worldBrick.ground = sand;
                    }
                    else
                    {
                        worldBrick.ground = grass;
                    }
                }
            }

            foreach (var coords in levelBounds.GetWorldCoords())
            {
                if (random.NextFloat() >= 1f / 30f)
                {
                    continue;
                }
                
                if (coords.y >= World.GroundHeight)
                {
                    continue;
                }

                Piece piece;

                if (coords.y > ForestHeight)
                {
                    piece = GetRandomPiece(desertPieces, random);
                }
                else
                {
                    piece = GetRandomPiece(forestPieces, random);
                }

                GeneratePiece(piece, level, coords.x, coords.y);
            }

            var powerX = random.NextInt(levelBounds.xMin, levelBounds.xMax);
            var powerY = random.NextInt(ForestHeight, -20);
            GeneratePiece(database.GetPieceWithTag("PowerGemEarth"), level, powerX, powerY, true);

            GeneratePiece(database.GetPieceWithTag("Golem"), level, powerX, 0, true);

            //GeneratePiece(database.GetPieceWithTag("YellowDungeon"), level, 0, 0, true);

            return level;
        }

        public Level GenerateDungeonLevel(string id, string pieceTag)
        {
            var database = _worldManager.database;
            var piece = database.GetPieceWithTag(pieceTag);
            
            var levelTop = piece.height - piece.pivot.y;
            var levelBottom = piece.pivot.y;
            var levelLeft = piece.pivot.x;
            var levelRight = piece.width - piece.pivot.x;
            
            var level = new Level(id, LevelType.YellowDungeon, levelTop, levelBottom, levelLeft, levelRight);
  
            GeneratePiece(piece, level, 0, 0, true);
            return level;
        }

        public void Generate()
        {
            var world = new World();

            world.levels.Add(GenerateMainLevel());
            world.levels.Add(GenerateDungeonLevel("YellowDungeon", "YellowDungeon"));

            WorldManager.world = world;
        }

        public void LoadWorld(WorldData worldData)
        {
            // var world = new World(worldData);
            //
            // _worldManager.world = world;
        }

        private void GeneratePiece(Piece piece, Level world, int worldX, int worldY, bool isOverlap = false)
        {
            for (var x = 0; x < piece.width; x++)
            {
                for (var y = 0; y < piece.height; y++)
                {
                    var coords = new Vector2Int(worldX + x, worldY + y) - piece.pivot;

                    var worldBrick = world.GetBrick(coords.x, coords.y, out _);

                    if (worldBrick is null || (!isOverlap && worldBrick.ground is null))
                    {
                        continue;
                    }

                    piece.GetBrick(x, y).CopyTo(worldBrick);
                    worldBrick.wall = wall;
                }
            }

            foreach (var prefab in piece.entities)
            {
                world.AddEntity(worldX - piece.pivot.x + prefab.x + 0.5f, worldY - piece.pivot.y + prefab.y + 0.5f,
                    prefab.origin);
            }
        }
    }
}