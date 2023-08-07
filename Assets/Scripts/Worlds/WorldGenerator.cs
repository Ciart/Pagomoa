using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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

            DataManager.Instance.LoadGameData();

            if (DataManager.Instance.data.worldData == null)
            {
                Debug.Log("worldData Not find, Generate New World");
                Generate();

            }
            else
            {
                if (DataManager.Instance.data.worldData._chunks.data.Count != 0)
                {
                    Debug.Log("worldData Exist, Load Old World");
                    LoadWorld(DataManager.Instance.data.worldData);
                }
                else
                {
                    Debug.Log("worldData Get Damaged, Generate New World");
                    Generate();
                }
            }
        }
        private void Preload()
        {
            var pieces = database.pieces;

            float weightCount = pieces.Sum(piece => piece.weight);

            _weightedPieces = new List<(float, Piece)>();

            foreach (var piece in pieces)
            {
                _weightedPieces.Add((piece.weight / weightCount, piece));
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

            var worldLeft = -left * chunkSize;
            var worldRight = right * chunkSize;
            var worldBottom = -bottom * chunkSize;
            var worldTop = top * chunkSize;

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
                            worldBrick.ground = ground;
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

                    var piece = GetRandomPiece();
                    GeneratePiece(piece, world, x, y);
                }
            }

            _worldManager.world = world;
        }
        public void LoadWorld(WorldData worldData)
        {
            Preload();

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