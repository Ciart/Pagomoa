using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds.UFO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    public class WorldManager : SingletonMonoBehaviour<WorldManager>
    {
        public WorldDatabase database;

        [FormerlySerializedAs("mineralEntity")]
        public ItemEntity itemEntity;

        public Transform ufo;

        public Tilemap ufoLadder;

        private UFOInteraction _ufoInteraction;

        private World _world;

        public static World world
        {
            get => instance._world;
            set
            {
                if (instance._world == value)
                {
                    return;
                }

                instance._world = value;
                EventManager.Notify(new WorldCreatedEvent(instance._world));
            }
        }

        private HashSet<Chunk> _expiredChunks = new();

        protected override void Awake()
        {
            base.Awake();

            _ufoInteraction = ufo.GetComponent<UFOInteraction>();
        }

        private void LateUpdate()
        {
            UpdateDiggingBrickDamage();

            if (_expiredChunks.Count == 0)
            {
                return;
            }

            foreach (var chunk in _expiredChunks)
            {
                // TODO: Level 값 다른 방식으로 변경
                EventManager.Notify(new ChunkChangedEvent(world.currentLevel, chunk));
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

        public bool CheckNull(Vector3 pos)
        {
            var p = ComputeCoords(pos);
            var brick = _world.currentLevel.GetBrick(p.x, p.y, out var chunk);
            if (brick?.ground is null)
                return true;
            else
                return false;
        }

        public Dictionary<BrickCoords, BrickHealth> brickDamage = new();
        private Dictionary<BrickCoords, float> diggingBrickDamage = new();

        private void UpdateDiggingBrickDamage()
        {
            var newBrickDamage = new Dictionary<BrickCoords, BrickHealth>();

            foreach (var (coords, damage) in brickDamage)
            {
                if (diggingBrickDamage.ContainsKey(coords))
                {
                    var brickHealth = brickDamage[coords];
                    brickHealth.health -= diggingBrickDamage[coords];

                    newBrickDamage.Add(coords, brickHealth);

                    diggingBrickDamage.Remove(coords);
                }
                else
                {
                    continue;
                }

                if (newBrickDamage[coords].health <= 0)
                {
                    BreakGround(coords.x, coords.y, 10);
                    newBrickDamage.Remove(coords);
                }
            }

            brickDamage = newBrickDamage;

            foreach (var (coords, damage) in diggingBrickDamage)
            {
                var brick = _world.currentLevel.GetBrick(coords.x, coords.y, out _);

                if (brick?.ground is null)
                {
                    continue;
                }

                var brickHealth = BrickHealth.FromBrick(brick);
                brickHealth.health -= damage;

                brickDamage.Add(coords, brickHealth);

                if (brickHealth.health <= 0)
                {
                    BreakGround(coords.x, coords.y, 10);
                    brickDamage.Remove(coords);
                }
            }

            diggingBrickDamage.Clear();
        }

        public void DigGround(BrickCoords coords, float digSpeed)
        {
            if (diggingBrickDamage.ContainsKey(coords))
            {
                diggingBrickDamage[coords] += digSpeed * Time.deltaTime;
            }
            else
            {
                diggingBrickDamage.Add(coords, digSpeed * Time.deltaTime);
            }
        }

        public void BreakGround(int x, int y, int tier, bool isForceBreak = false)
        {
            var brick = _world.currentLevel.GetBrick(x, y, out var chunk);
            var rock = database.GetMineral("Rock");

            if (chunk is null)
                return;

            if (brick.mineral is not null && brick.mineral.tier <= tier)
            {
                if (!isForceBreak && brick.mineral == rock)
                {
                    return;
                }

                if (brick.mineral != rock)
                {
                    var entity = Instantiate(itemEntity, ComputePosition(x, y), Quaternion.identity);
                    entity.Item = brick.mineral!.item;
                }
            }

            var prevBrick = (Brick)brick.Clone();
            
            Debug.Log(prevBrick.mineral?.displayName);

            brick.ground = null;
            brick.mineral = null;

            foreach (var c in _world.currentLevel.GetNeighborChunks(chunk.key))
            {
                _expiredChunks.Add(c);
            }

            EventManager.Notify(new GroundBrokenEvent(x, y, prevBrick));
        }

        public bool CheckBreakable(int x, int y, int tier, string item)
        {
            var brick = _world.currentLevel.GetBrick(x, y, out var chunk);
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
                    if (chunk is null) return false;
                    if (brick.mineral?.displayName == "돌") return false;
                }

                if (brick.mineral is not null && brick.mineral.tier <= tier)
                    return true;
            }

            return true;
        }

        public bool CheckClimbable(Vector3 position)
        {
            var coords = ComputeCoords(position);
            var brick = _world.currentLevel.GetBrick(coords.x, coords.y, out _);

            var ladderPos = ufoLadder.WorldToCell(new Vector3(position.x, position.y - 1f));
            var ladder = ufoLadder.GetTile<TileBase>(ladderPos);

            return (brick?.wall is not null && brick.wall.isClimbable) || ladder is not null;
        }

        public void MoveUfoBase()
        {
            if (_ufoInteraction.canMove)
            {
                _ufoInteraction.canMove = false;
                StartCoroutine(_ufoInteraction.MoveToPlayer());
            }
        }
    }
}