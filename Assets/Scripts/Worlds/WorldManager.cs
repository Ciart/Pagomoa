using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Worlds
{
    public class WorldManager : MonoBehaviour
    {
        public MineralEntity mineralEntity;

        public Tilemap ufoLadder;
        
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

        private static WorldManager _instance;

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

        public void BreakGround(int x, int y, int tier, string item)
        {
            var brick = _world.GetBrick(x, y, out var chunk);
            if (item == "item") 
            {
                if(chunk is null)
                    return;

                if (brick.mineral is not null && brick.mineral.tier <= tier && brick.mineral?.displayName != "돌")
                {
                    var entity = Instantiate(mineralEntity, ComputePosition(x, y), Quaternion.identity);
                    entity.Data = brick.mineral;
                }
            }
            else
            {
                if (chunk is null || brick.mineral?.displayName == "돌")
                    return;

                if (brick.mineral is not null && brick.mineral.tier <= tier)
                {
                    var entity = Instantiate(mineralEntity, ComputePosition(x, y), Quaternion.identity);
                    entity.Data = brick.mineral;
                }
            }
            //if (chunk is null || brick.mineral?.displayName == "돌")
            //{
            //    return;
            //}

            //if (brick.mineral is not null && brick.mineral.tier <= tier)
            //{
            //    var entity = Instantiate(mineralEntity, ComputePosition(x, y), Quaternion.identity);
            //    entity.Data = brick.mineral;
            //}

            brick.ground = null;
            brick.mineral = null;

            // _expiredChunks.Add(chunk);

            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    var c = _world.GetChunk(chunk.key + new Vector2Int(i, j));

                    if (c is null)
                    {
                        continue;
                    }
                    
                    _expiredChunks.Add(c);
                }
            }
        }
        public bool CheckBreakable(int x, int y, int tier, string item)
        {
            var brick = _world.GetBrick(x, y, out var chunk);
            if (item == "item")
            {
                if (chunk is null) return false;
                if (brick.mineral is not null && brick.mineral.tier <= tier && brick.mineral?.displayName != "돌")
                    return true;
            }
            else
            {
                if (chunk is null || brick.mineral?.displayName == "돌")
                {
                    if(chunk is null) return false;
                    if(brick.mineral?.displayName == "돌") return false;
                }
                if (brick.mineral is not null && brick.mineral.tier <= tier)
                    return true;
            }
            return true;
        }
        public bool CheckClimbable(Vector3 position)
        {
            var coords = ComputeCoords(position);
            var brick = _world.GetBrick(coords.x, coords.y, out _);

            var ladderPos = ufoLadder.WorldToCell(new Vector3(position.x, position.y - 1f));
            var ladder = ufoLadder.GetTile<TileBase>(ladderPos);

            return (brick?.wall is not null && brick.wall.isClimbable) || ladder is not null;
        }
    }
}