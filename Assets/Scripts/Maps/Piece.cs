using System;
using UnityEngine;

namespace Maps
{
    [Serializable]
    public class Piece
    {
        public int width = 2;
        
        public int height = 2;

        [SerializeField]
        private Brick[] _bricks;
        
        public Vector2Int Pivot = Vector2Int.zero;

        public int Rarity = 1;

        public Piece()
        {
            _bricks = new Brick[width * height];
        }

        public void ResizeBricks()
        {
            Array.Resize(ref _bricks, width * height);
        }

        public ref Brick GetBrick(int x, int y)
        {
            return ref _bricks[GetBricksIndex(x, y)];
        }

        public void SetBrick(Brick brick, int x, int y)
        {
            _bricks[GetBricksIndex(x, y)] = brick;
        }

        private int GetBricksIndex(int x, int y)
        {
            return x + y * width;
        }
    }
}