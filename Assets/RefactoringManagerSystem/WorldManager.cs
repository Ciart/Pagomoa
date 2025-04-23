using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using UnityEngine;


namespace Ciart.Pagomoa.Worlds
{
    public class WorldManager : Manager<WorldManager>
    {
        public WorldGenerator worldGenerator;

        public WorldRenderer worldRenderer;

        public WorldDatabase database;

        public ItemEntity itemEntity;

        private World _world;

        public static World world
        {
            get => Game.Instance.World._world;
            private set
            {
                if (Game.Instance.World._world == value)
                {
                    return;
                }

                Game.Instance.World._world = value;
                EventManager.Notify(new WorldCreatedEvent(Game.Instance.World._world));
            }
        }

        private HashSet<Chunk> _expiredChunks = new();

        public WorldManager()
        {
            EventManager.AddListener<DataSaveEvent>(OnDataSaveEvent);
            EventManager.AddListener<DataLoadedEvent>(OnDataLoadedEvent);

            worldGenerator = new WorldGenerator();
            worldRenderer = Object.Instantiate(DataBase.data.GetWorldRenderer());

            database = DataBase.data.GetWorldData();
            itemEntity = DataBase.data.GetItemEntity();
        }

        private void OnDataSaveEvent(DataSaveEvent e)
        {
            e.saveData.world = world.CreateSaveData();
        }

        private void OnDataLoadedEvent(DataLoadedEvent e)
        {
            world = new World(e.saveData.world);
        }

        public override void Start()
        {
            if (world is not null)
            {
                return;
            }

            world = worldGenerator.Generate();
        }

        public override void OnDestroy()
        {
            EventManager.RemoveListener<DataSaveEvent>(OnDataSaveEvent);
            EventManager.RemoveListener<DataLoadedEvent>(OnDataLoadedEvent);
        }

        public override void LateUpdate()
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
        public static Vector3 ComputePosition(WorldCoords coords)
        {
            return ComputePosition(coords.x, coords.y);
        }

        /// <summary>
        /// Scene의 Global 위치를 World의 좌표로 변환
        /// </summary>
        /// <param name="position">Scene의 Global 위치</param>
        /// <returns>World 좌표</returns>
        public static WorldCoords ComputeCoords(Vector3 position)
        {
            return new WorldCoords(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
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

        public Dictionary<WorldCoords, BrickHealth> brickDamage = new();
        private Dictionary<WorldCoords, float> diggingBrickDamage = new();

        private void UpdateDiggingBrickDamage()
        {
            var newBrickDamage = new Dictionary<WorldCoords, BrickHealth>();

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

        public void DigGround(WorldCoords coords, float digSpeed)
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

            if (chunk is null)
                return;

            if (brick.mineral is not null && brick.mineral.tier <= tier)
            {
                if (!isForceBreak && brick.isRock)
                {
                    return;
                }

                if (!brick.isRock)
                {
                    var entity = itemEntity.InstantiateItem(itemEntity, ComputePosition(x, y));
                    entity.Item = brick.mineral!.item;
                }
            }

            var prevBrick = (Brick)brick.Clone();

            brick.groundId = null;
            brick.mineralId = null;

            foreach (var c in _world.currentLevel.GetNeighborChunks(chunk.coords))
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
                if (brick.mineral is not null && brick.mineral.tier <= tier && brick.mineral?.name != "돌")
                    return true;
            }
            else
            {
                if (chunk is null || brick.mineral?.name == "돌")
                {
                    if (chunk is null) return false;
                    if (brick.mineral?.name == "돌") return false;
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

            /*var ladderPos = ufoLadder.WorldToCell(new Vector3(position.x, position.y - 1f));
            var ladder = ufoLadder.GetTile<TileBase>(ladderPos);*/

            return (brick?.wall is not null && brick.wall.isClimbable); // || ladder is not null;
        }

        public bool IsBrickAboveGround(int targetX, int targetY)
        {
            var targetBrick = _world.currentLevel.GetBrick(targetX, targetY, out var notUseChunk1);

            if (targetBrick.ground != null) return false;

            var targetGroundBrick = _world.currentLevel.GetBrick(targetX, targetY - 1, out var notUseChunk2);

            return targetGroundBrick.ground != null;
        }

        public Vector2Int GetClosestAboveEmptyGroundVector(float basePosX, float basePosY, int searchRange,
            int minSearchRange = 0)
        {
            var closeBricks = new List<Vector2Int>();

            var searchCount = 0;

            var x = Mathf.FloorToInt(basePosX);
            var y = Mathf.FloorToInt(basePosY);

            var initVector = new Vector2Int(-1, 1);
            var intPos = new Vector2Int(x, y);

            var xPlusCount = 2 + 2 * minSearchRange;
            var xMinusCount = -2 - 2 * minSearchRange;
            var yPlusCount = 2 + 2 * minSearchRange;
            var yMinusCount = -2 - 2 * minSearchRange;

            var startPos = intPos + initVector * minSearchRange;

            while (searchCount < searchRange - minSearchRange)
            {
                startPos += initVector;

                var targetVector = startPos;

                for (int xp = 0; xp < xPlusCount; xp++)
                {
                    targetVector += new Vector2Int(1, 0);

                    if (IsBrickAboveGround(targetVector.x, targetVector.y))
                    {
                        var targetBrick = new Vector2Int(targetVector.x, targetVector.y);
                        closeBricks.Add(targetBrick);
                    }
                }

                for (int ym = 0; ym > yMinusCount; ym--)
                {
                    targetVector += new Vector2Int(0, -1);

                    if (IsBrickAboveGround(targetVector.x, targetVector.y))
                    {
                        var targetBrick = new Vector2Int(targetVector.x, targetVector.y);
                        closeBricks.Add(targetBrick);
                    }
                }

                for (int xm = 0; xm > xMinusCount; xm--)
                {
                    targetVector += new Vector2Int(-1, 0);

                    if (IsBrickAboveGround(targetVector.x, targetVector.y))
                    {
                        var targetBrick = new Vector2Int(targetVector.x, targetVector.y);
                        closeBricks.Add(targetBrick);
                    }
                }

                for (int yp = 0; yp < yPlusCount; yp++)
                {
                    targetVector += new Vector2Int(0, 1);

                    if (IsBrickAboveGround(targetVector.x, targetVector.y))
                    {
                        var targetBrick = new Vector2Int(targetVector.x, targetVector.y);
                        closeBricks.Add(targetBrick);
                    }
                }

                if (closeBricks.Count > 0)
                {
                    var targetPos = new Vector2(basePosX, basePosY);
                    var distances = new Dictionary<float, Vector2Int>();

                    foreach (var vector in closeBricks)
                    {
                        var distance = Vector2.Distance(targetPos, vector);
                        distances.Add(distance, vector);
                    }

                    var minDistance = distances.Keys.Min();

                    return distances[minDistance];
                }

                searchCount++;

                xPlusCount += 2;
                xMinusCount -= 2;
                yPlusCount += 2;
                yMinusCount -= 2;
            }

            // 주변에 가까운 블럭이 없으면 매개변수를 다시 반환
            return new Vector2Int((int)basePosX, (int)basePosY);
        }

        public List<Vector2Int> GetAboveEmptyGroundVectors(int basePosX, int basePosY, int searchRange,
            int minSearchRange = 0)
        {
            var onGroundList = new List<Vector2Int>();

            var xRange = 0;
            var yRange = 0;

            xRange = basePosX + searchRange;
            yRange = basePosY - searchRange;

            for (var x = basePosX + 1; x < xRange; x++)
            {
                for (var y = basePosY; y > yRange; y--)
                {
                    if (x <= basePosX + minSearchRange && y >= basePosY - minSearchRange) continue;
                    if (!IsBrickAboveGround(x, y)) continue;

                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }

            xRange = basePosX - searchRange;
            yRange = basePosY + searchRange;

            for (var x = basePosX - 1; x > xRange; x--)
            {
                for (var y = basePosY; y < yRange; y++)
                {
                    if (x >= basePosX - minSearchRange && y <= basePosY + minSearchRange) continue;
                    if (!IsBrickAboveGround(x, y)) continue;

                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }

            xRange = basePosX + searchRange;
            yRange = basePosY + searchRange;

            for (var y = basePosY + 1; y < yRange; y++)
            {
                for (var x = basePosX; x < xRange; x++)
                {
                    if (x <= basePosX + minSearchRange && y <= basePosY + minSearchRange) continue;
                    if (!IsBrickAboveGround(x, y)) continue;

                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }

            xRange = basePosX - searchRange;
            yRange = basePosY - searchRange;

            for (var y = basePosY - 1; y > yRange; y--)
            {
                for (var x = basePosX; x > xRange; x--)
                {
                    if (x >= basePosX - minSearchRange && y >= basePosY - minSearchRange) continue;
                    if (!IsBrickAboveGround(x, y)) continue;

                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }

            return onGroundList;
        }

        public void MoveUfoBase()
        {
            /*if (_ufoInteraction.canMove)
            {
                _ufoInteraction.canMove = false;
                StartCoroutine(_ufoInteraction.MoveToPlayer());
            }*/
        }

        public async Awaitable WaitForLevelUpdate()
        {
            while (true)
            {
                if (!worldRenderer.GetLevelRenderer(world.currentLevel).IsLoading)
                {
                    return;
                }

                await Awaitable.NextFrameAsync();
            }
        }
    }
}
