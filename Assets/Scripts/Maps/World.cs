using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Maps
{
    public class Chunk
    {
        public readonly Vector2Int Key;

        public Brick[,] Bricks;

        public Chunk(Vector2Int key, int size)
        {
            Key = key;
            Bricks = new Brick[size, size];
        }
    }

    public class World
    {
        public readonly int ChunkSize;

        public readonly int Top;

        public readonly int Bottom;

        public readonly int Left;

        public readonly int Right;

        public readonly int GroundHeight = 0;

        private readonly Dictionary<Vector2Int, Chunk> _chunks;

        public World(int chunkSize, int top, int bottom, int left, int right)
        {
            ChunkSize = chunkSize;
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;

            _chunks = new Dictionary<Vector2Int, Chunk>();

            for (var i = -Top; i < Bottom; i++)
            {
                for (var j = -Left; j < Right; j++)
                {
                    var key = new Vector2Int(i, j);
                    
                    _chunks[key] = new Chunk(key, ChunkSize);
                }
            }
        }

        public Brick GetBrick(Vector2Int coordinates, out Chunk chunk)
        {
            chunk = _chunks[coordinates / ChunkSize];

            return chunk.Bricks[coordinates.x % ChunkSize, coordinates.y % ChunkSize];
        }

        public Brick GetBrick(Vector2Int coordinates)
        {
            return GetBrick(coordinates, out _);
        }

        public Chunk GetChunk(Vector2Int key)
        {
            return _chunks[key];
        }
    }
}