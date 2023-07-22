using System;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds
{
    [Serializable]
    public struct PiecePrefabItem
    {
        public int x;
        public int y;
        public GameObject prefab;

        public PiecePrefabItem(int x, int y, GameObject prefab)
        {
            this.x = x;
            this.y = y;
            this.prefab = prefab;
        }
    }

    [Serializable]
    public class Piece
    {
        public int width = 2;

        public int height = 2;

        public Vector2Int pivot = Vector2Int.zero;

        public int rarity = 1;

        [SerializeField] private Brick[] _bricks;

        [SerializeField] private List<PiecePrefabItem> _prefabs = new();

        public Piece()
        {
            _bricks = new Brick[width * height];
        }

        public void ResizeBricks()
        {
            Array.Resize(ref _bricks, width * height);

            _prefabs = _prefabs.FindAll(item => CheckRange(item.x, item.y));
        }

        public Brick GetBrick(int x, int y)
        {
            return _bricks[GetBrickIndex(x, y)];
        }

        private int GetBrickIndex(int x, int y)
        {
            return x + y * width;
        }

        public bool CheckRange(int x, int y)
        {
            return 0 <= x && x < width && 0 <= y && y < height;
        }

        public void AddPrefab(int x, int y, GameObject prefab)
        {
            if (!CheckRange(x, y))
            {
                return;
            }

            _prefabs.Add(new PiecePrefabItem(x, y, prefab));
        }
    }
}