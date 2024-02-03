using System;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Chunk
    {
        public const int Size = 16;
        
        // TODO: key 대신 다른 단어로 교체해야 함.
        public Vector2Int key;

        public Brick[] bricks;

        public readonly Rect worldRect;

        public Chunk(Vector2Int key)
        {
            this.key = key;
            bricks = new Brick[Size * Size];
            worldRect = new Rect(key.x * Size, key.y * Size, Size, Size);

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    bricks[i + j * Size] = new Brick();
                }
            }
        }
        
        private bool CheckRange(float x, float y)
        {
            return 0 <= x && x < Size && 0 <= y && y < Size;
        }
    }
}