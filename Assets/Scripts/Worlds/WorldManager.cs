using System;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds
{
    public class WorldManager : MonoBehaviour
    {
        public MineralEntity mineralEntity;

        private World _world;

        public World world
        {
            get => _world;
            set
            {
                if (_world == value)
                {
                    return;
                }

                _world = value;
                createdWorld?.Invoke(_world);
            }
        }

        public event Action<World> createdWorld;

        public event Action<Chunk> changedChunk;

        private static WorldManager _instance = null;

        private HashSet<Chunk> _expiredChunks = new();

        public static WorldManager instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (WorldManager)FindObjectOfType(typeof(WorldManager));
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void LateUpdate()
        {
            if (_expiredChunks.Count == 0)
            {
                return;
            }

            foreach (var chunk in _expiredChunks)
            {
                changedChunk?.Invoke(chunk);
            }

            _expiredChunks.Clear();
        }

        public static Vector3 ComputePosition(int x, int y)
        {
            return new Vector3(x + 0.5f, y + 0.5f, 0);
        }

        /// <summary>
        /// World 좌표를 Scene의 Global 위치로 변환
        /// </summary>
        /// <param name="coords">World 좌표</param>
        /// <returns>Scene의 Global 위치</returns>
        public static Vector3 ComputePosition(Vector2Int coords)
        {
            return ComputePosition(coords.x, coords.y);
        }

        /// <summary>
        /// Scene의 Global 위치를 World의 좌표로 변환
        /// </summary>
        /// <param name="position">Scene의 Global 위치</param>
        /// <returns>World 좌표</returns>
        public static Vector2Int ComputeCoords(Vector3 position)
        {
            return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        }

        public void BreakGround(int x, int y, int tier)
        {
            var brick = _world.GetBrick(x, y, out var chunk);

            if (chunk is null)
            {
                return;
            }

            if (brick.mineral is not null && brick.mineral.tier <= tier)
            {
                var entity = Instantiate(mineralEntity, ComputePosition(x, y), Quaternion.identity);
                entity.Data = brick.mineral;
            }

            brick.ground = null;
            brick.mineral = null;

            _expiredChunks.Add(chunk);
        }

        public bool CheckClimbable(Vector3 position)
        {
            var coords = ComputeCoords(position);
            var brick = _world.GetBrick(coords.x, coords.y, out _);

            if (brick is null && brick.wall is null)
            {
                return false;
            }

            return brick.wall.isClimbable;
        }
    }
}