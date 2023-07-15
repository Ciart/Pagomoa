using System;
using System.Collections;
using System.Collections.Generic;
using Maps;
using UnityEngine;

public class NightMonsterSpawner : MonoBehaviour
{
    public Transform player;
    
    public bool x, y, z;
    
    public int land;

    [SerializeField] private int _maxSpawn = 10;
    
    [SerializeField] private float _spawnTerm = 30f;
    
    [SerializeField] private float _spawnCoolTime = 30f;

    [SerializeField] private GameObject[] _monsterType;

    private GameObject _monsterPrefab;

    private List<GameObject> _spawnedMonster;

    private Vector3Int _spawnPoint;



    private float _desertDepth = 50f;
    private float _forestDepth = -100f;

    private int _boxSize = 10;
    private int _checkVectorX = - 1;

    void Start()
    {
        _spawnedMonster = new List<GameObject>();

        SetSpawnMonster();
        CheckSpawnPosition();
    }

    public void StartNightSpawner()
    {
        ChasePlayer();
        if (_spawnCoolTime <= 0)
        {
            _spawnCoolTime = _spawnTerm;
        }
        if (_spawnCoolTime == _spawnTerm)
        {
            StartCoroutine(nameof(SpawnMonsters));
        }
        if (_spawnCoolTime <= _spawnTerm)
        {
            _spawnCoolTime -= 0.05f;
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
                _monsterPrefab = _monsterType[land - 1];
                break;

            case float yPos when yPos > _forestDepth:
                land = 2;
                _monsterPrefab = _monsterType[land - 1];
                break;

            case float yPos when yPos > _forestDepth:
                land = 3;
                _monsterPrefab = _monsterType[land - 1];
                break;

            case float yPos when yPos > _forestDepth:
                land = 4;
                _monsterPrefab = _monsterType[land - 1];
                break;

            case float yPos when yPos > _forestDepth:
                land = 5;
                _monsterPrefab = _monsterType[land - 1];
                break;

            case float yPos when yPos > _forestDepth:
                land = 6;
                _monsterPrefab = _monsterType[land - 1];
                break;
        }
    }
    
    private IEnumerator SpawnMonsters()
    {
        if (_spawnedMonster.Count < _maxSpawn)
        {
            CheckSpawnPosition();
            
            GameObject nightMonster = Instantiate(_monsterPrefab, _spawnPoint, Quaternion.identity);
            _spawnedMonster.Add(nightMonster);
            
            yield return new WaitForSeconds(_spawnTerm);
        }
        yield return null;
        SetSpawnMonster();
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
        float bottomLeftY = transform.position.y - _boxSize / 2 - 1;
        float topRightY = transform.position.y + _boxSize / 2 - 1;

        if (_checkVectorX > 0)
        {
            SearchLeftTiles(bottomLeftY, topRightY);
        }
        else if (_checkVectorX < 0)
        {
            SearchRightTiles(bottomLeftY, topRightY);
        }
    }

    private void SearchLeftTiles(float bottomLeftY, float topRightY)
    {
        var mapManager = MapManager.Instance;
        
        float bottomLeftX = transform.position.x - 10f;
        float topRightX = transform.position.x - 3f;
        
        for (float x = bottomLeftX; x <= topRightX; x++)
        {
            for (float y = bottomLeftY; y <= topRightY; y++)
            {
                Vector3Int tilePosition = mapManager.groundTilemap.layoutGrid.WorldToCell(new Vector3(x, y));
                var tile = mapManager.GetBrick(tilePosition).ground;

                if (!tile)
                {
                    Vector3Int isNullTilePos = mapManager.groundTilemap.layoutGrid.WorldToCell(new Vector3(x, y - 1f));
                    var tileCheck = mapManager.GetBrick(isNullTilePos).ground;

                    if (tileCheck)
                    {
                        _spawnPoint = tilePosition;
                        _checkVectorX = -1;
                        return;
                    }
                }
            }
        }
    }

    private void SearchRightTiles(float bottomLeftY, float topRightY)
    {
        var mapManager = MapManager.Instance;
                    
        float bottomLeftX = transform.position.x + 3f;
        float topRightX = transform.position.x + 10f;
            
        for (float x = topRightX; x >= bottomLeftX; x--)
        {
            for (float y = bottomLeftY; y <= topRightY; y++)
            {
                Vector3Int tilePosition = mapManager.groundTilemap.layoutGrid.WorldToCell(new Vector3(x, y));
                var tile = mapManager.GetBrick(tilePosition).ground;
                        
                if (!tile)
                {
                    Vector3Int isNullTilePos = mapManager.groundTilemap.layoutGrid.WorldToCell(new Vector3(x, y - 1f));
                    var tileCheck = mapManager.GetBrick(isNullTilePos).ground;

                    if (tileCheck)
                    {
                        _spawnPoint = tilePosition;
                        _checkVectorX = 1;
                        return ;
                    }
                }
            }
        }
    }
}