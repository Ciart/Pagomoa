using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Ciart.Pagomoa.Worlds
{
    using WeightedPieces = List<(float, Piece)>;

    [RequireComponent(typeof(WorldManager))]
    public class WorldGenerator : MonoBehaviour
    {
        public const int ForestHeight = -200;

        public bool isTestWorld = false;

        public uint seed = 1234;

        public int chunkSize = 16;

        public int top = 4;

        public int bottom = 8;

        public int left = 4;

        public int right = 4;

        public Wall wall;

        private WorldManager _worldManager;

        private void Awake()
        {
            _worldManager = GetComponent<WorldManager>();

            if (isTestWorld)
            {
                top = 4;
                bottom = 4;
                left = 4;
                right = 4;
            }
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

            var worldLeft = -left * chunkSize;
            var worldRight = right * chunkSize;
            var worldBottom = -bottom * chunkSize;
            var worldTop = top * chunkSize;

            var sand = database.GetGround("Sand");
            var grass = database.GetGround("Grass");

            for (var x = worldLeft; x < worldRight; x++)
            {
                for (var y = worldBottom; y < worldTop; y++)
                {
                    if (y >= World.GroundHeight)
                    {
                        continue;
                    }


                    var worldBrick = level.GetBrick(x, y, out _);

                    if (worldBrick is not null)
                    {
                        worldBrick.wall = wall;

                        if (y > ForestHeight)
                        {
                            worldBrick.ground = sand;
                        }
                        else
                        {
                            worldBrick.ground = grass;
                        }
                    }
                }
            }

            for (var x = worldLeft; x < worldRight; x++)
            {
                for (var y = worldBottom; y < worldTop; y++)
                {
                    if (random.NextFloat() >= 1f / 30f)
                    {
                        continue;
                    }

                    Piece piece;

                    if (y > ForestHeight)
                    {
                        piece = GetRandomPiece(desertPieces, random);
                    }
                    else
                    {
                        piece = GetRandomPiece(forestPieces, random);
                    }

                    GeneratePiece(piece, level, x, y);
                }
            }

            var powerX = random.NextInt(worldLeft, worldRight);
            var powerY = random.NextInt(ForestHeight, 100);
            GeneratePiece(database.GetPieceWithTag("PowerGemEarth"), level, powerX, powerY, true);

            GeneratePiece(database.GetPieceWithTag("Remote"), level, 0, -4, true);

            return level;
        }

        public Level GenerateDungeonLevel()
        {
            var level = new Level("Dungeon", LevelType.YellowDungeon, 1, 1, 1, 1);
            
            var levelLeft = -left * chunkSize;
            var levelRight = right * chunkSize;
            var levelBottom = -bottom * chunkSize;
            var levelTop = top * chunkSize;
            
            var sand = _worldManager.database.GetGround("Sand");
            
            for (var x = levelLeft; x < levelRight; x++)
            {
                for (var y = levelBottom; y < levelTop; y++)
                {
                    if (y >= World.GroundHeight)
                    {
                        continue;
                    }


                    var worldBrick = level.GetBrick(x, y, out _);

                    if (worldBrick is not null)
                    {
                        worldBrick.wall = wall;
                        worldBrick.ground = sand;
                    }
                }
            }

            return level;
        }

        public void Generate()
        {
            var world = new World();

            world.levels.Add(GenerateMainLevel());
            world.levels.Add(GenerateDungeonLevel());

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