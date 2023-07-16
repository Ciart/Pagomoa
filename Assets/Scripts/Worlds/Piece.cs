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

        [SerializeField]
        private Brick[] _bricks;

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
            return ref _bricks[GetBrickIndex(x, y)];
        }
        
        private int GetBrickIndex(int x, int y)
        {
            return x + y * width;
        }
    }
}