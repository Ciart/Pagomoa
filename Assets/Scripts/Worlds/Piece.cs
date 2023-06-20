using System;
using UnityEngine;

namespace Worlds
{
    [Serializable]
    public class Piece
    {
        public int width = 2;
        
        public int height = 2;
        
        public Vector2Int pivot = Vector2Int.zero;
        
        public int rarity = 1;

        private Brick[] _bricks;

        public Piece()
        {
            _bricks = new Brick[width * height];
        }

        public void ResizeBricks()
        {
            Array.Resize(ref _bricks, width * height);
        }

        public Brick GetBrick(int x, int y)
        {
            return _bricks[GetBricksIndex(x, y)];
        }
        
        private int GetBricksIndex(int x, int y)
        {
            return x + y * width;
        }
    }
}