using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public class NightMonsterSpawner : MonoBehaviour
    {
        public Transform player;
    
        public bool x, y, z;
    
        public int land;

        public int minSearchWidth = 4;
    
        public int maxSearchWidth = 10;

        [SerializeField] private int maxSpawn = 10;
    
        [SerializeField] private float spawnTerm = 30f;
    
        [SerializeField] private float spawnCoolTime = 30f;

        [SerializeField] private GameObject[] monsterType;

        private GameObject _monsterPrefab;

        private List<GameObject> _spawnedMonster;

        private List<WorldCoords> _canSpawnPoints;
    
        private Vector2 _spawnPoint;

        private float _desertDepth = 50f;
        private float _forestDepth = -100f;

        private int _searchSize = 10;
        //private int _checkVectorX = - 1;

        void Start()
        {
            _spawnedMonster = new List<GameObject>();
            _canSpawnPoints = new List<WorldCoords>();

            SetSpawnMonster();
            CheckSpawnPosition();
        }

        public void StartNightSpawner()
        {
            ChasePlayer();
            if (spawnCoolTime <= 0)
            {
                spawnCoolTime = spawnTerm;
            }
            if (spawnCoolTime == spawnTerm)
            {
                StartCoroutine(nameof(SpawnMonsters));
            }
            if (spawnCoolTime <= spawnTerm)
            {
                spawnCoolTime -= 0.05f;
            }
        }

        private void ChasePlayer()
        {
            if (!player) return ;
        
            transform.position = new Vector3(
                (x ? player.position.x : transform.position.x),
                (y ? player.position.y : transform.position.y),
                (z ? player.position.z : transform.position.z)); 
        }

        private void SetSpawnMonster()
        {
            float playerPositionY = player.transform.position.y;

            switch (playerPositionY)
            {
                case float yPos when yPos > _desertDepth:
                    land = 1;
                    _monsterPrefab = monsterType[land - 1];
                    break;

                case float yPos when yPos > _forestDepth:
                    land = 2;
                    _monsterPrefab = monsterType[land - 1];
                    break;

                case float yPos when yPos > _forestDepth:
                    land = 3;
                    _monsterPrefab = monsterType[land - 1];
                    break;

                case float yPos when yPos > _forestDepth:
                    land = 4;
                    _monsterPrefab = monsterType[land - 1];
                    break;

                case float yPos when yPos > _forestDepth:
                    land = 5;
                    _monsterPrefab = monsterType[land - 1];
                    break;

                case float yPos when yPos > _forestDepth:
                    land = 6;
                    _monsterPrefab = monsterType[land - 1];
                    break;
            }
        }
    
        private IEnumerator SpawnMonsters()
        {
            if (_spawnedMonster.Count < maxSpawn)
            {
                SetSpawnMonster();
                CheckSpawnPosition();
            
                int randomPoint = UnityEngine.Random.Range(0, _canSpawnPoints.Count);
                var coords = _canSpawnPoints[randomPoint];
                _spawnPoint = new Vector2(coords.x, coords.y);
                _spawnPoint.y += 0.5f;
            

                GameObject nightMonster = Instantiate(_monsterPrefab, _spawnPoint, Quaternion.identity);
                _spawnedMonster.Add(nightMonster);
            
                yield return new WaitForSeconds(spawnTerm);
            }
            yield return null;
        }

        public void KillNightMonsters()
        {
            foreach (var nightMonster in _spawnedMonster)
            {
                Destroy(nightMonster);
            }
            _spawnedMonster.Clear();
        }
    
        private void CheckSpawnPosition()
        {
            Vector3 spawnerPivot = transform.position;

            Vector2Int bottomLeft = InitBottomLeftPosition(spawnerPivot);
            Vector2Int topRight = InitTopRightPosition(spawnerPivot);
            bottomLeft.x += minSearchWidth;
            topRight.x += maxSearchWidth;

            SearchTiles(bottomLeft, topRight);

            bottomLeft = InitBottomLeftPosition(spawnerPivot);
            topRight = InitTopRightPosition(spawnerPivot);
            bottomLeft.x -= maxSearchWidth;
            topRight.x -= minSearchWidth;

            SearchTiles(bottomLeft, topRight);
        }

        private void SearchTiles(Vector2Int bottomLeft, Vector2Int topRight)
        {
            for (int x = bottomLeft.x; x <= topRight.x; x++)
            {
                for (int y = bottomLeft.y; y <= topRight.y; y++)
                {
                    var tilePosition = WorldManager.ComputeCoords(new Vector3(x, y));
                    var tile = WorldManager.world.currentLevel.GetBrick(tilePosition.x, tilePosition.y, out _)?.ground;

                    if (tile == null)
                    {
                        var isNullTilePos =  WorldManager.ComputeCoords(new Vector3(x, y - 1f));
                        var tileCheck = WorldManager.world.currentLevel.GetBrick(isNullTilePos.x, isNullTilePos.y, out _)?.ground;

                        if (tileCheck != null)
                        {
                            _canSpawnPoints.Add(tilePosition);
                        }
                    }
                }
            }
        }

        private Vector2Int InitBottomLeftPosition(Vector3 spawnerPivot)
        {
            return new Vector2Int(
                Mathf.FloorToInt(spawnerPivot.x),
                Mathf.FloorToInt(spawnerPivot.y) - _searchSize / 2 - 1
            );
        }

        private Vector2Int InitTopRightPosition(Vector3 spawnerPivot)
        {
            return new Vector2Int(
                Mathf.CeilToInt(spawnerPivot.x),
                Mathf.CeilToInt(spawnerPivot.y) + _searchSize / 2 - 1
            );
        }
    }
}