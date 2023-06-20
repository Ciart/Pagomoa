using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Worlds
{
    public class Chunk
    {
        public readonly Vector2Int key;

        public Brick[,] bricks;

        public Chunk(Vector2Int key, int size)
        {
            this.key = key;
            bricks = new Brick[size, size];
        }
    }

    public class World
    {
        public readonly int chunkSize;

        public readonly int top;

        public readonly int bottom;

        public readonly int left;

        public readonly int right;

        public readonly int groundHeight = 0;

        private readonly Dictionary<Vector2Int, Chunk> _chunks;

        public World(int chunkSize, int top, int bottom, int left, int right)
        {
            this.chunkSize = chunkSize;
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;

            _chunks = new Dictionary<Vector2Int, Chunk>();

            for (var i = -this.top; i < this.bottom; i++)
            {
                for (var j = -this.left; j < this.right; j++)
                {
                    var key = new Vector2Int(i, j);

                    _chunks[key] = new Chunk(key, this.chunkSize);
                }
            }
        }

        public Brick GetBrick(Vector2Int coordinates, out Chunk chunk)
        {
            if (_chunks.TryGetValue(coordinates / chunkSize, out chunk))
            {
                var x = coordinates.x < 0 ? chunkSize - 1 + coordinates.x % chunkSize : coordinates.x % chunkSize;
                var y = coordinates.y < 0 ? chunkSize - 1 + coordinates.y % chunkSize : coordinates.y % chunkSize;

                return chunk.bricks[x, y];
            }

            return null;
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