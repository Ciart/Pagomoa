using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Worlds
{
    [Serializable]
    public class Piece
    {
        public int width = 2;

        public int height = 2;

        public Vector2Int pivot = Vector2Int.zero;

        public float weight = 1;

        public List<WorldPrefab> prefabs = new();

        [SerializeField] private Brick[] _bricks;

        public Piece()
        {
            _bricks = new Brick[width * height];
        }

        public void ResizeBricks()
        {
            Array.Resize(ref _bricks, width * height);

            prefabs = prefabs.FindAll(item => CheckRange(item.x, item.y));
        }

        public Brick GetBrick(int x, int y)
        {
            return _bricks[GetBrickIndex(x, y)];
        }

        private int GetBrickIndex(int x, int y)
        {
            return x + y * width;
        }

        private bool CheckRange(float x, float y)
        {
            return 0 <= x && x < width && 0 <= y && y < height;
        }

        public void AddPrefab(int x, int y, GameObject prefab)
        {
            if (!CheckRange(x, y))
            {
                return;
            }

            prefabs.Add(new WorldPrefab(x, y, prefab));
        }
    }
}