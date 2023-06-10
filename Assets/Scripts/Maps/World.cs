using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    public class Chunk
    {
        public Brick[,] Bricks;

        public Chunk(int size)
        {
            Bricks = new Brick[size, size];
        }
    }

    public class World
    {
        public readonly int ChunkSize = 16;

        public readonly int Top = 8;

        public readonly int Bottom = 32;

        public readonly int Left = 8;

        public readonly int Right = 8;

        public readonly int GroundHeight = 0;

        private Dictionary<Vector2Int, Chunk> _chunks;

        public World()
        {
            _chunks = new Dictionary<Vector2Int, Chunk>();

            for (var i = -Top; i < Bottom; i++)
            {
                for (var j = -Left; j < Right; j++)
                {
                    _chunks[new Vector2Int(i, j)] = new Chunk(ChunkSize);
                }
            }
        }

        public Brick GetBrick(Vector2Int position)
        {
            var chunk = _chunks[position / ChunkSize];

            return chunk.Bricks[position.x % ChunkSize, position.y % ChunkSize];
        }

        // public Brick SetBrick(int x, int y, Brick brick)
        // {
        //     
        // }
    }
}