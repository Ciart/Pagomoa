using System;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Worlds
{
    public class WorldManager : MonoBehaviour
    {
        public MineralEntity mineralEntity;

        public World world;

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
                _expiredChunks.Remove(chunk);
            }
        }

        /// <summary>
        /// World 좌표를 Scene의 Global 위치로 변환
        /// </summary>
        /// <param name="coordinates">World 좌표</param>
        /// <returns>Scene의 Global 위치</returns>
        public static Vector3 ComputePosition(Vector2Int coordinates)
        {
            return new Vector3(coordinates.x + 0.5f, coordinates.y + 0.5f, 0);
        }

        /// <summary>
        /// Scene의 Global 위치를 World의 좌표로 변환
        /// </summary>
        /// <param name="position">Scene의 Global 위치</param>
        /// <returns>World 좌표</returns>
        public static Vector2Int ComputeCoordinates(Vector3 position)
        {
            return new Vector2Int((int)position.x, (int)position.y);
        }

        public void BreakGround(Vector2Int coordinates, int tier = 10)
        {
            var brick = world.GetBrick(coordinates, out var chunk);

            if (brick.mineral.tier <= tier)
            {
                var entity = Instantiate(mineralEntity, ComputePosition(coordinates),
                    Quaternion.identity);
                entity.Data = brick.mineral;
            }

            brick.ground = null;
            brick.mineral = null;

            _expiredChunks.Add(chunk);
        }

        public bool CheckClimbable(Vector3 position)
        {
            var brick = world.GetBrick(ComputeCoordinates(position));

            return brick.wall.isClimbable;
        }
    }
}