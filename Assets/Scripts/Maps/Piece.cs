using System;
using UnityEngine;

namespace Maps
{
    public class Piece
    {
        private int _width = 2;

        public int Width
        {
            get => _width;
            set
            {
                _width = value;

                Bricks = ResizeArray(Bricks, Width, Height);
            }
        }

        private int _height = 2;
        
        public int Height
        {
            get => _height;
            set
            {
                _height = value;

                Bricks = ResizeArray(Bricks, Width, Height);
            }
        }

        public Brick[,] Bricks;
        
        public Vector2Int Pivot = Vector2Int.zero;

        public int Rarity = 1;

        public Piece()
        {
            Bricks = new Brick[Width, Height];
        }
        
        private static T[,] ResizeArray<T>(T[,] original, int x, int y)
        {
            var newArray = new T[x, y];
            var minX = Math.Min(original.GetLength(0), newArray.GetLength(0));
            var minY = Math.Min(original.GetLength(1), newArray.GetLength(1));

            for (var i = 0; i < minY; i++)
            {
                Array.Copy(original, i * original.GetLength(0), newArray, i * newArray.GetLength(0), minX);
            }

            return newArray;
        }
    }
}