using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public class NightMonsterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject monster;
        [SerializeField] private float cycleTime = 5f;
        [SerializeField] private int maxCount = 5;
        private List<EntityController> monsters = new List<EntityController>();

        [SerializeField] private Vector2Int spawnRange = new Vector2Int(25, 15);
        [SerializeField] private Vector2Int spawnExclusionRange = new Vector2Int(20, 10);

        private TimeManager _timeManager;

        #region Debugging

        Vector2Int debugPoint;
        [SerializeField] List<Vector2Int> debugVectors;

        #endregion

        private void Awake()
        {
            _timeManager = TimeManager.instance;
        }

        private void Start()
        {
            if (monster is not null)
                StartCoroutine(StartSpawn());
        }

        private void SpawnMonster(GameObject monster)
        {
            if (monsters.Count >= maxCount) return;
            if (!(_timeManager.hour < 6 || _timeManager.hour >= 18)) return;
            Debug.Log("소환" + TimeManager.instance.hour + ":" + TimeManager.instance.minute);

            var entityManager = EntityManager.instance;

            var entityId = monster.GetComponent<EntityController>().entityId;

            var player = Game.instance.player;
            var playerPosition = player.transform.position;

            var basePoint = new Vector2Int((int)playerPosition.x, (int)playerPosition.y);

            var spawnPoints = BrickSearchUtility.GetAboveEmptyGroundVectors(
                basePoint
                , spawnRange
                , spawnExclusionRange);

            var point = spawnPoints[Random.Range(0, spawnPoints.Count)];

            var spawnedMonster = entityManager.Spawn(entityId, new Vector3(point.x, point.y));

            spawnedMonster.TakeDamage(0, 0, player.GetComponent<EntityController>(), DamageFlag.Melee);
            spawnedMonster.died += e => { RemoveMonster(spawnedMonster); };

            monsters.Add(spawnedMonster);

            // just For Debugging
            // ForDebug(basePoint, spawnPoints);
        }

        private void RemoveMonster(EntityController entity)
        {
            monsters.Remove(entity);
        }


        private IEnumerator StartSpawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(cycleTime);
                SpawnMonster(monster);
            }
        }


        #region Debugging

        private void ForDebug(Vector2Int basePoint, List<Vector2Int> points)
        {
            debugPoint = basePoint;
            debugVectors = points;
        }

        private void OnDrawGizmos()
        {
            if (debugVectors.Count == 0) return;
            if (monsters.Count >= maxCount) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(debugPoint.x, debugPoint.y),
                new Vector3(spawnRange.x * 2, spawnRange.y * 2));
            Gizmos.DrawWireCube(new Vector3(debugPoint.x, debugPoint.y),
                new Vector3(spawnExclusionRange.x * 2, spawnExclusionRange.y * 2));


            foreach (var intPos in debugVectors)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(new Vector3(intPos.x, intPos.y), new Vector3(intPos.x + 1, intPos.y));
                Gizmos.DrawCube(new Vector3(intPos.x + 0.5f, intPos.y + 0.5f), new Vector3(0.5f, 0.5f));
            }

            Gizmos.color = Color.green;

            Vector2Int targetBrickVector2Int = Vector2Int.zero;

            Gizmos.DrawCube(new Vector3(targetBrickVector2Int.x + 0.5f, targetBrickVector2Int.y + 0.5f),
                new Vector3(0.3f, 0.3f));
        }

        #endregion
    }
}