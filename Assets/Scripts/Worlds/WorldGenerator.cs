using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Worlds
{
    using WeightedPieces = List<(float, Piece)>;

    [RequireComponent(typeof(WorldManager))]
    public class WorldGenerator : MonoBehaviour
    {
        public WorldDatabase database;

        public int chunkSize = 32;

        public int top = 4;

        public int bottom = 32;

        public int left = 4;

        public int right = 4;

        public Wall wall;

        public Ground ground;

        private WorldManager _worldManager;

        private void Awake()
        {
            _worldManager = GetComponent<WorldManager>();
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

        private Piece GetRandomPiece(WeightedPieces weightedPieces)
        {
            var pivot = Random.value;

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

        public void Generate()
        {
            var desertPieces =
                Preload(database.pieces.Where((piece) => piece.appearanceArea.HasFlag(WorldAreaFlag.Desert)));
            var forestPieces =
                Preload(database.pieces.Where((piece) => piece.appearanceArea.HasFlag(WorldAreaFlag.Forest)));

            var world = new World(chunkSize, top, bottom, left, right);

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
                    if (y >= world.groundHeight)
                    {
                        continue;
                    }


                    var worldBrick = world.GetBrick(x, y, out _);

                    if (worldBrick is not null)
                    {
                        worldBrick.wall = wall;

                        if (y > -50)
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
                    if (Random.Range(0, 30) != 0)
                    {
                        continue;
                    }

                    Piece piece;

                    if (y > -50)
                    {
                        piece = GetRandomPiece(desertPieces);
                    }
                    else
                    {
                        piece = GetRandomPiece(forestPieces);
                    }

                    GeneratePiece(piece, world, x, y);
                }
            }

            _worldManager.world = world;
        }

        public void LoadWorld(WorldData worldData)
        {
            var world = new World(worldData);

            var worldLeft = -worldData.left * chunkSize;
            var worldRight = worldData.right * chunkSize;
            var worldBottom = -worldData.bottom * chunkSize;
            var worldTop = worldData.top * chunkSize;

            for (var x = worldLeft; x < worldRight; x++)
            {
                for (var y = worldBottom; y < worldTop; y++)
                {
                    if (y >= world.groundHeight)
                    {
                        continue;
                    }

                    {
                        var worldBrick = world.GetBrick(x, y, out _);
                        if (worldBrick is not null)
                        {
                            worldBrick.wall = wall;
                            if (worldBrick.ground != null)
                                worldBrick.ground = ground;
                            //Debug.Log((x - 16) + "/" + (y - 16) + "/" + world.chunkSize);
                        }
                    }
                }
            }

            _worldManager.world = world;
        }

        private void GeneratePiece(Piece piece, World world, int worldX, int worldY)
        {
            for (var x = 0; x < piece.width; x++)
            {
                for (var y = 0; y < piece.height; y++)
                {
                    var coords = new Vector2Int(worldX + x, worldY + y) - piece.pivot;

                    var worldBrick = world.GetBrick(coords.x, coords.y, out _);

                    if (worldBrick is null || worldBrick.ground is null)
                    {
                        continue;
                    }

                    piece.GetBrick(x, y).CopyTo(worldBrick);
                    worldBrick.wall = wall;
                }
            }

            foreach (var prefab in piece.prefabs)
            {
                world.AddPrefab(worldX + prefab.x, worldY + prefab.y, prefab.prefab);
            }
        }
    }
}