using System;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds
{
    public class Chunk
    {
        public readonly Vector2Int key;

        public readonly Brick[] bricks;

        public Chunk(Vector2Int key, int size)
        {
            this.key = key;
            bricks = new Brick[size * size];

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    bricks[i + j * size] = new Brick();
                }
            }
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

            for (var keyX = -this.left; keyX < this.right; keyX++)
            {
                for (var keyY = -this.bottom; keyY < this.top; keyY++)
                {
                    var key = new Vector2Int(keyX, keyY);
                    _chunks[key] = new Chunk(key, this.chunkSize);
                }
            }
        }

        public Chunk GetChunk(Vector2Int key)
        {
            return _chunks.TryGetValue(key, out var chunk) ? chunk : null;
        }

        public Chunk GetChunk(int x, int y)
        {
            var key = new Vector2Int(Mathf.FloorToInt((float)x / chunkSize), Mathf.FloorToInt((float)y / chunkSize));

            return GetChunk(key);
        }

        public Brick GetBrick(int x, int y, out Chunk chunk)
        {
            chunk = GetChunk(x, y);

            if (chunk is null)
            {
                return null;
            }

            var brinkX = x < 0 ? chunkSize - 1 + (x + 1) % chunkSize  : x % chunkSize;
            var brinkY = y < 0 ? chunkSize - 1 + (y + 1) % chunkSize : y % chunkSize;

            return chunk.bricks[brinkX + brinkY * chunkSize];
        }
    }
}